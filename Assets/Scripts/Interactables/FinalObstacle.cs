using System;
using System.Collections;
using System.Collections.Generic;
using Audio.FMOD;
using Dog;
using Ghosts;
using UnityEngine;

namespace Interactables
{
    public class FinalObstacle : Obstacle
    {
        [SerializeField] private List<GhostMovement> _ghosts;
        
        [SerializeField] private PlayerMove _player;
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private DogWaitForPlayer _wait;
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _tree;
        [SerializeField] private GameObject _callBlock;
        [SerializeField] private GameObject _dogDead;
        
        private List<GhostAttack> _ghostsAttackDog = new List<GhostAttack>();
        private List<GhostAttack> _ghostsAttackPlayer = new List<GhostAttack>();
        
        private List<Vector3> _initialGhostsPositions = new List<Vector3>();

        private Coroutine _stopKeepingDistance;
        private Coroutine _slowMotion;

        private PlayerStateManager _playerState;

        private void Start()
        {
            _playerState = _player.GetComponent<PlayerStateManager>();
            
            _ghostsAttackPlayer.Add(_ghosts[^1].GetComponent<GhostAttack>());

            for (int j = 0; j < _ghosts.Count - 1; j++)
            {
                _ghostsAttackDog.Add(_ghosts[j].GetComponent<GhostAttack>());
            }
            
            foreach (var ghost in _ghosts)
            {
                _initialGhostsPositions.Add(ghost.transform.position);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player.UpdatePlayerRunning(true);
                AudioManager.Instance.SetFloatParameter(AudioManager.Instance.musicInstance,
                    "Ending Run", 2, false);
            }
            else if (other.CompareTag("Dog"))
            {
                _dog.Running(true);
            }
        }

        public void StartChasing()
        {
            foreach (var ghost in _ghostsAttackDog)
            {
                ghost.Attack(_dog.transform);
            }
            
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.Attack(_player.transform);
            }
        }

        public void AttackDog()
        {
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.StopAttacking(false, Vector3.zero);
                ghost.Attack(_dog.transform);
            }

            StartCoroutine(TreeFall());
        }

        private IEnumerator TreeFall()
        {
            yield return new WaitForSeconds(0.5f);
            
            _tree.transform.rotation = Quaternion.Euler(0, 0, 121f);
            _tree.transform.localPosition = new Vector3(-0.5f, 0.73f, 0);
            // TODO Play tree fall sound
            
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DogDie, 
                _dog.transform.position,
                true);
            
            // turn off all ghosts and turn on ghost
            foreach (var ghost in _ghosts)
            {
                ghost.gameObject.SetActive(false);
            }
            
            // turn off dog
            _dog.gameObject.SetActive(false);
            _dogDead.SetActive(true);
        }

        public void ActivateSlowMotion()
        {
            _slowMotion = StartCoroutine(SlowMotion());
        }
        
        private IEnumerator SlowMotion()
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForSecondsRealtime(4f);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        
        public void StopKeepingDistance()
        {
            foreach (var ghost in _ghostsAttackDog)
            {
                ghost.SetKeepDistance(false);
                ghost.SetAttackSpeed(6.5f);
            }
            
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.SetKeepDistance(false);
                ghost.SetAttackSpeed(6.5f);
            }
        }

        public void TurnOffCallBlock()
        {
            _callBlock.SetActive(false);
            StartCoroutine(WaitToBeSad());
        }

        private IEnumerator WaitToBeSad()
        {
            yield return new WaitForSeconds(1f);
            
            _playerState.UpdateIsSad(true);
        }
        
        public override void ResetObstacle()
        {
            if (_stopKeepingDistance != null) StopCoroutine(_stopKeepingDistance);
            if (_slowMotion != null)
            {
                StopCoroutine(_slowMotion);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
            
            foreach (var ghost in _ghostsAttackDog)
            {
                ghost.StopAttacking(false, Vector3.zero);
                ghost.SetKeepDistance(true);
                ghost.SetAttackSpeed(5f);
            }
            
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.StopAttacking(false, Vector3.zero);
                ghost.SetKeepDistance(true);
                ghost.SetAttackSpeed(5f);
            }
            
            for (int i = 0; i < _ghosts.Count; i++)
            {
                _ghosts[i].transform.position = _initialGhostsPositions[i];
            }
            
            _wall.SetActive(false);
            _player.UpdatePlayerRunning(false);
            _dog.Running(false);

            _wait.PlayerReached();
        }
    }
}
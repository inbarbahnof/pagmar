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
        [SerializeField] private Transform[] _ghostPosToGo;
        [SerializeField] private Transform[] _playerGhostsGoTo;
        
        [SerializeField] private PlayerMove _player;
        [SerializeField] private DogActionManager _dog;
        [SerializeField] private DogWaitForPlayer _wait;
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _tree;
        [SerializeField] private GameObject _callBlock;
        [SerializeField] private GameObject _dogDead;
        [SerializeField] private GameObject _moveToMainCamera;

        [Header("Initial Ghosts")]
        [SerializeField] private GhostAttack _playerInitialGhost;
        [SerializeField] private GhostAttack _dogInitialGhost;
        [SerializeField] private GameObject _parentGameObject;
        
        [Header("Player or Dog Attack")]
        [SerializeField] private List<GhostAttack> _ghostsAttackDog = new List<GhostAttack>();
        [SerializeField] private List<GhostAttack> _ghostsAttackPlayer = new List<GhostAttack>();
        
        private List<Vector3> _initialGhostsPositions = new List<Vector3>();
        private List<PlayGhostieSound> _ghostSounds;

        private Coroutine _stopKeepingDistance;
        private Coroutine _slowMotion;

        private PlayerStateManager _playerState;

        private void Start()
        {
            _playerState = _player.GetComponent<PlayerStateManager>();
            _ghostSounds = new List<PlayGhostieSound>();
            
            foreach (var ghost in _ghosts)
            {
                _initialGhostsPositions.Add(ghost.transform.position);
                _ghostSounds.Add(ghost.GetComponent<PlayGhostieSound>());
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player.UpdatePlayerRunning(true);
                // AudioManager.Instance.SetFloatParameter(AudioManager.Instance.musicInstance,
                //     "Ending Run", 2, false);
                
                AudioManager.Instance.PlayMusic(FMODEvents.Instance.Chapter4EndMusic);
                AudioManager.Instance.MuteAmbienceEvent();
                
                _dog.Running(true);
                
                _parentGameObject.SetActive(true);
                
                _playerInitialGhost.Attack(_player.transform);
                _dogInitialGhost.Attack(_dog.transform);
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
            GameManager.instance.Chapter4();
            
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

            foreach (var sound in _ghostSounds)
            {
                sound.GhostEndParameter();
            }
            
            for (int i = 0; i < _ghostsAttackDog.Count; i++)
            {
                _ghostsAttackDog[i].StopAttacking(false, _dog.transform.position);
                GhostMovement movement = _ghostsAttackDog[i].GetComponent<GhostMovement>();
                if (movement != null)
                {
                    movement.GoToTargetAndPause(_ghostPosToGo[i]);
                }
            }
            
            for (int i = 0; i < _ghostsAttackPlayer.Count; i++)
            {
                _ghostsAttackPlayer[i].StopAttacking(false, _player.transform.position);
                GhostMovement movement = _ghostsAttackPlayer[i].GetComponent<GhostMovement>();
                if (movement != null)
                {
                    movement.GoToTargetAndPause(_playerGhostsGoTo[i]);
                }
            }

            StartCoroutine(WaitToAttackPlayer());
        }

        private IEnumerator WaitToAttackPlayer()
        {
            yield return new WaitForSecondsRealtime(3f);
            
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.Attack(_player.transform);
                ghost.SetAttackSpeed(11f);
            }
        }
        
        private IEnumerator SlowMotion()
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForSecondsRealtime(5f);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        
        public void StopKeepingDistance()
        {
            foreach (var ghost in _ghostsAttackPlayer)
            {
                ghost.SetKeepDistance(false);
                ghost.SetAttackSpeed(6.5f);
            }
        }

        public void TurnOnCameraChangeTrigger()
        {
            _moveToMainCamera.SetActive(true);
        }

        public void TurnOffCallBlock()
        {
            _callBlock.SetActive(false);
            GameManager.instance.PlayVolumeEffect();
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
            
            for (int i = 0; i < _ghostsAttackDog.Count - 1; i++)
            {
                _ghostsAttackDog[i].StopAttacking(false, Vector3.zero);
                _ghostsAttackDog[i].SetKeepDistance(true);
                _ghostsAttackDog[i].SetAttackSpeed(5f);
            }
            _ghostsAttackDog[^1].StopAttacking(false, Vector3.zero);

            for (int i = 0; i < _ghostsAttackPlayer.Count - 1; i++)
            {
                _ghostsAttackPlayer[i].StopAttacking(false, Vector3.zero);
                _ghostsAttackPlayer[i].SetKeepDistance(true);
                _ghostsAttackPlayer[i].SetAttackSpeed(5f);
            }
            _ghostsAttackPlayer[^1].StopAttacking(false, Vector3.zero);
            _ghostsAttackPlayer[^1].SetAttackSpeed(6.3f);
            
            for (int i = 0; i < _ghosts.Count; i++)
            {
                _ghosts[i].transform.position = _initialGhostsPositions[i];
            }
            
            foreach (var sound in _ghostSounds)
            {
                sound.ResetGhostEndParameter();
            }
            
            // AudioManager.Instance.ResumeAmbience();
            AudioManager.Instance.StopMusic();
            print("stop music");
            
            _wall.SetActive(false);
            _player.UpdatePlayerRunning(false);
            _dog.Running(true);

            _wait.FinishTargetActionWithoutReach();
        }
    }
}
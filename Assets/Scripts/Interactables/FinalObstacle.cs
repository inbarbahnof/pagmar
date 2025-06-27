using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private List<GhostAttack> _ghostsAttackDog = new List<GhostAttack>();
        private List<GhostAttack> _ghostsAttackPlayer = new List<GhostAttack>();
        
        private List<Vector3> _initialGhostsPositions = new List<Vector3>();

        private Coroutine _stopKeepingDistance;
        private Coroutine _slowMotion;

        private void Start()
        {
            int i = _ghosts.Count / 2;

            for (int j = 0; j < i; j++)
            {
                _ghostsAttackDog.Add(_ghosts[j].GetComponent<GhostAttack>());
            }
            
            for (int k = i; k < _ghosts.Count; k++)
            {
                _ghostsAttackPlayer.Add(_ghosts[k].GetComponent<GhostAttack>());
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
        }

        private IEnumerator WaitToStopKeepingDistance()
        {
            yield return new WaitForSeconds(0.2f);
            
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

        public void ActivateSlowMotion()
        {
            _slowMotion = StartCoroutine(SlowMotion());
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
            
            // if (_stopKeepingDistance != null) StopCoroutine(_stopKeepingDistance);
            // _stopKeepingDistance = StartCoroutine(WaitToStopKeepingDistance());
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
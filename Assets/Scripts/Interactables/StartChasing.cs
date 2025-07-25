using System;
using System.Collections;
using UnityEngine;

namespace Interactables
{
    public class StartChasing : MonoBehaviour
    {
        [SerializeField] private FinalObstacle _finalObstacle;
        [SerializeField] private bool _chasingBoth = true;
        [SerializeField] private GameObject _wall;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_chasingBoth)
                    _finalObstacle.StartChasing();
                else
                {
                    _finalObstacle.AttackDog();
                    CameraController.instance.ExtraZoomIn();
                    
                    Collider2D col = GetComponent<Collider2D>();
                    col.enabled = false;
                    
                    StartCoroutine(WaitToActivateWall());
                }
            }
        }

        private IEnumerator WaitToActivateWall()
        {
            yield return new WaitForSeconds(1f);
            if (_wall != null) _wall.SetActive(true);
        }
    }
}
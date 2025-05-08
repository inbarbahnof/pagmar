using System;
using Targets;
using UnityEngine;

namespace CheckpointUtils
{
    public class TurnOnFood : MonoBehaviour
    {
        [SerializeField] private FoodTarget _foodTarget;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_foodTarget != null)
            {
                _foodTarget.SetCanBeFed(true);
            }
        }
    }
}

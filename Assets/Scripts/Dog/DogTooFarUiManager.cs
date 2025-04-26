using System;
using UnityEngine;

namespace Dog
{ 
    public class DogTooFarUiManager : MonoBehaviour
    {
        [SerializeField] private DogActionManager _dogActionManager;
        [SerializeField] private GameObject ui;

        private void Update()
        {
            if (_dogActionManager.DogPlayerDistance <= _dogActionManager.ListenDistance)
            {
                if (ui.activeSelf)
                {
                    ui.SetActive(false);
                }
            }
            else
            {
                if (!ui.activeSelf)
                {
                    ui.SetActive(true);
                }
            }
        }
    }
}

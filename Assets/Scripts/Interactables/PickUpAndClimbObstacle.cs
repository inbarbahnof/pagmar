using System;
using System.Collections;
using Interactabels;
using Interactables;
using UnityEngine;

public class PickUpAndClimbObstacle : Obstacle
{
    [SerializeField] private PickUpInteractableManager pickUpManager;
    [SerializeField] private GameObject roadBlock;
    private bool _setupComplete;
    private Vector2 _carryTarget;

    void Start()
    {
        _carryTarget = transform.position;
        pickUpManager.SetCarryTarget(_carryTarget, ReachedTarget);
    }

    public void ReachedTarget()
    {
        _setupComplete = true;
        roadBlock.GetComponent<Collider2D>().enabled = false;
    }
    
}

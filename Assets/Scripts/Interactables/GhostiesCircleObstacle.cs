using System.Collections.Generic;
using Dog;
using Ghosts;
using Interactables;
using Targets;
using UnityEngine;

public class GhostiesCircleObstacle : Obstacle
{
    [SerializeField] private ThrowablePickUpInteractable[] _foodInteractables;
    [SerializeField] private FoodTarget[] _food;
    [SerializeField] private PushObstacle _pushObs;
    [SerializeField] private DogActionManager _dog;
    [SerializeField] private GhostieMovement[] _ghosties;
    [SerializeField] private PlayerStateManager _player;

    private Vector3[] _foodPositions;
    private Dictionary<Transform, int> _foodTransformToIndex;

    private void Start()
    {
        _foodPositions = new Vector3[_foodInteractables.Length];

        _foodTransformToIndex = new Dictionary<Transform, int>();

        for (int i = 0; i < _foodInteractables.Length; i++)
        {
            _foodPositions[i] = _food[i].transform.position;
            
            ThrowablePickUpInteractable interactable = _foodInteractables[i];
            interactable.OnThrowComplete += ThrowComplete;

            _foodTransformToIndex[_foodInteractables[i].transform] = i;
        }
    }

    private void ThrowComplete(Transform pos)
    {
        if (_foodTransformToIndex.TryGetValue(pos, out int index))
        {
            _dog.FoodIsClose(pos.GetComponent<Collider2D>());
            _food[index].SetCanBeFed(true);
        }
    }

    // private void RespawnFood(int index)
    // {
    //     ThrowablePickUpInteractable oldFood = _spawnedFoods[index];
    //
    //     GameObject newFood = Instantiate(oldFood.gameObject, _foodPositions[index], Quaternion.identity);
    //     FoodTargetGetter foodTargetGetter = newFood.GetComponent<FoodTargetGetter>();
    //     FoodTarget foodTarget = foodTargetGetter.GetFoodTarget();
    //
    //     foodTarget.SetCanBeFed(false);
    //     foodTarget.OnFoodEaten += () => RespawnFood(index);
    //
    //     _spawnedFoods[index] = newFood.GetComponent<ThrowablePickUpInteractable>();
    // }

    public void GhostiesRun()
    {
        foreach (var ghosty in _ghosties)
        {
            ghosty.MoveAwayFromDog(_dog.transform.position);
        }
    }

    public override void ResetObstacle()
    {
        for (int i = 0; i < _food.Length; i++)
        {
            // Reactivate the original food
            _foodInteractables[i].ResetState(_foodPositions[i], transform);

            // Also re-enable art & collider
            _foodInteractables[i].GetComponent<Collider2D>().enabled = true;
            _food[i].SetCanBeFed(false);
            _food[i].GetPickup().gameObject.SetActive(true);
        }

        foreach (var ghostie in _ghosties)
        {
            ghostie.ResetMovement();
        }

        PickUpInteractableManager.instance.DropObject();
        PushInteractableManager.instance.StopPush();

        _pushObs.ResetObstacle();
        _player.OnFinishedInteraction(PickUpInteractableManager.instance.CurPickUp);
    }
}
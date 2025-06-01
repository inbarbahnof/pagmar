using System.Collections.Generic;
using Dog;
using Ghosts;
using Interactables;
using Targets;
using UnityEngine;

public class GhostiesCircleObstacle : Obstacle
{
    [SerializeField] private FoodTarget[] _food;
    [SerializeField] private Obstacle _pushObs;
    [SerializeField] private DogActionManager _dog;
    [SerializeField] private GhostieMovement[] _ghosties;

    private Vector3[] _foodPositions;
    private FoodTarget[] _spawnedFoods;
    private Dictionary<Transform, int> _foodTransformToIndex;

    private void Start()
    {
        _foodPositions = new Vector3[_food.Length];
        _spawnedFoods = new FoodTarget[_food.Length];

        _foodTransformToIndex = new Dictionary<Transform, int>();

        for (int i = 0; i < _food.Length; i++)
        {
            _foodPositions[i] = _food[i].transform.position;

            int index = i;
            _food[i].OnFoodEaten += () => RespawnFood(index);
            ThrowablePickUpInteractable interactable = _food[i].GetComponent<ThrowablePickUpInteractable>();
            interactable.OnThrowComplete += ThrowComplete;

            _foodTransformToIndex[_food[i].transform] = i;
            _spawnedFoods[i] = _food[i];
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

    private void RespawnFood(int index)
    {
        FoodTarget oldFood = _spawnedFoods[index];

        GameObject newFood = Instantiate(oldFood.gameObject, _foodPositions[index], Quaternion.identity);
        FoodTarget foodTarget = newFood.GetComponent<FoodTarget>();

        foodTarget.SetCanBeFed(false);
        foodTarget.OnFoodEaten += () => RespawnFood(index);

        _spawnedFoods[index] = foodTarget;
    }

    public void GhostiesRun()
    {
        foreach (var ghosty in _ghosties)
        {
            ghosty.MoveAwayFromDog(_dog.transform.position);
        }
    }

    public override void ResetObstacle()
    {
        for (int i = 0; i < _foodPositions.Length; i++)
        {
            if (_spawnedFoods[i] != null)
            {
                _spawnedFoods[i].transform.position = _foodPositions[i];
                _spawnedFoods[i].gameObject.SetActive(true);
                _spawnedFoods[i].SetCanBeFed(false);
                _spawnedFoods[i].GetComponent<Collider2D>().enabled = true;
            }
        }

        _pushObs.ResetObstacle();
    }
}
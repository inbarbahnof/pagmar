using Interactables;
using Targets;
using UnityEngine;

public class GhostiesCircleObstacle : Obstacle
{
    [SerializeField] private FoodTarget[] _food;
    [SerializeField] private Obstacle _pushObs;

    private Vector3[] _foodPositions;
    private FoodTarget[] _spawnedFoods;

    private void Start()
    {
        _foodPositions = new Vector3[_food.Length];
        _spawnedFoods = new FoodTarget[_food.Length];

        for (int i = 0; i < _food.Length; i++)
        {
            _foodPositions[i] = _food[i].transform.position;

            int index = i;
            _food[i].OnFoodEaten += () => RespawnFood(index);

            _spawnedFoods[i] = _food[i];
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
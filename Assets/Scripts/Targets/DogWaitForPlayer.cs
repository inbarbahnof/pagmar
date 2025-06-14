using Dog;
using Interactables;
using Targets;
using UnityEngine;

public class DogWaitForPlayer : MonoBehaviour
{
    [SerializeField] private WantFoodTarget _wantFoodTarget;
    [SerializeField] private bool _isRunning;
    [SerializeField] private RunFromGhosties _runFromGhosties;
    [SerializeField] private FinalObstacle _finalObstacle;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
            
            DogActionManager dog = other.GetComponent<DogActionManager>();
            dog.SetWantsFood(true);
            
            if (_isRunning) dog.Running(true);
            
            // if (_runFromGhosties != null) _runFromGhosties.StartsRunning();
            if (_finalObstacle != null) _finalObstacle.StopKeepingDistance();
        }
    }

    public void PlayerReached()
    {
        _wantFoodTarget.FinishTargetAction();
    }
}

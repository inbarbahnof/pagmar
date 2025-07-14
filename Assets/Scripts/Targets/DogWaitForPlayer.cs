using System.Collections;
using Dog;
using Interactables;
using Targets;
using UnityEngine;

public class DogWaitForPlayer : MonoBehaviour
{
    [SerializeField] private WantFoodTarget _wantFoodTarget;
    [SerializeField] private bool _isRunning;
    [SerializeField] private FinalObstacle _finalObstacle;

    private bool _pullReachedTarget;
    private bool _playerReached;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog") && !_playerReached &&!_pullReachedTarget)
        {
            print("dog want food");
            DogActionManager dog = other.GetComponent<DogActionManager>();
            
            if (_finalObstacle != null)
                StartCoroutine(TurnAroundLastObs(dog));

            TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
            dog.SetWantsFood(true);
            
            if (_isRunning) dog.Running(true);
        }
    }

    public void GoToWait(DogActionManager dog)
    {
        if (_playerReached) return;
        TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
        dog.SetWantsFood(true);
    }

    private IEnumerator TurnAroundLastObs(DogActionManager dog)
    {
        dog.Growl(_wantFoodTarget.transform, false);
        yield return new WaitForSeconds(0.7f);
        
        dog.Bark();
        _finalObstacle.StopKeepingDistance();
        
        yield return new WaitForSeconds(0.5f);

        TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
        dog.SetWantsFood(true);
    }

    public void PlayerReached()
    {
        _playerReached = true;
        _wantFoodTarget.FinishTargetAction();
    }

    public void FinishTargetActionWithoutReach()
    {
        _wantFoodTarget.FinishTargetAction();
    }

    public void PullReachedTarget()
    {
        _pullReachedTarget = true;
    }
}

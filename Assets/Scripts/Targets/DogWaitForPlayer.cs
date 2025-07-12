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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            DogActionManager dog = other.GetComponent<DogActionManager>();
            
            if (_finalObstacle != null)
                StartCoroutine(TurnAroundLastObs(dog));

            TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
            dog.SetWantsFood(true);
            
            if (_isRunning) dog.Running(true);
        }
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
        _wantFoodTarget.FinishTargetAction();
    }
}

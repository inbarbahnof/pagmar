using Dog;
using Targets;
using UnityEngine;

public class DogWaitForPlayer : MonoBehaviour
{
    [SerializeField] private WantFoodTarget _wantFoodTarget;
    [SerializeField] private bool _isRunning;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            print("wait for player");
            TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
            
            DogActionManager dog = other.GetComponent<DogActionManager>();
            dog.SetWantsFood(true);
            if (_isRunning) dog.Running();
        }
    }

    public void PlayerReached()
    {
        _wantFoodTarget.FinishTargetAction();
    }
}

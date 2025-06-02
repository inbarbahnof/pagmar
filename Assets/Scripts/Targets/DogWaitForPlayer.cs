using Dog;
using Targets;
using UnityEngine;

public class DogWaitForPlayer : MonoBehaviour
{
    [SerializeField] private WantFoodTarget _wantFoodTarget;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            print("wait for player");
            TargetGenerator.instance.SetWantFoodTarget(_wantFoodTarget);
            other.GetComponent<DogActionManager>().SetWantsFood(true);
        }
    }

    public void PlayerReached()
    {
        _wantFoodTarget.FinishTargetAction();
    }
}

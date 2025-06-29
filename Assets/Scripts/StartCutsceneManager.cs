using Dog;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector sequence;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private DogActionManager dog;
    [SerializeField] private bool stopMove = true;
    
    public void ShowSequence()
    {
        // activate upon player reach start pos
        if (stopMove)
        {
            // freeze player
            FreezePositions();
        }
        
        // play sequence to pan camera right and move dog
        sequence.Play();
        // on camera pan stop allow player controls and show 'hide' prompt
    }

    public void FreezePositions()
    {
        playerMove.SetCanMove(false);
        dog.SetMovementEnabled(false);
    }

    public void AfterSequence()
    {
        playerMove.SetCanMove(true);
        dog.SetMovementEnabled(true);
    }
}
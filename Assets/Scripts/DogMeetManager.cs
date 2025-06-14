using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class DogMeetManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector dogSequence;
    [SerializeField] private PlayableDirector ghostieSequence;
    
    public void ShowDogSequence()
    {
        // activate upon player reach start pos
        // freeze player
        // play sequence to pan camera right and move dog
        dogSequence.Play();
        // on camera pan stop allow player controls and show 'hide' prompt
    }

    public void ShowGhostiesSequence()
    {
        // activate upon player reached bush
        // freeze player
        // play sequence to pam cam left and activate ghosties them move dog and scatter
        ghostieSequence.Play();
    }
}

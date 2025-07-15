using System.Collections;
using UnityEngine;

public class TurnOnTOIs : MonoBehaviour
{
    [SerializeField] private GameObject _TOIs;

    void Start()
    {
        StartCoroutine(WaitToTurnOnTOIs());
    }

    private IEnumerator WaitToTurnOnTOIs()
    {
        yield return new WaitForSeconds(12);
        _TOIs.SetActive(true);
    }
    
}

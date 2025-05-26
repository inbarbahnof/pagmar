using System.Collections;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private float showTime = 5f;
    private bool _showing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_showing && other.CompareTag("Player"))
        {
            _showing = true;
            StartCoroutine(ShowTxtForTime());
        }
    }

    private IEnumerator ShowTxtForTime()
    {
        Debug.Log("aaaaa");
        text.SetActive(true);
        yield return new WaitForSeconds(showTime);
        text.SetActive(false);
        _showing = false;
    }
}
using System.Collections;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject secondText;
    [SerializeField] private float showTime = 5f;
    private bool _showing = false;
    private int _textsShown = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_showing && other.CompareTag("Player"))
        {
            _showing = true;
            _textsShown++;
            StartCoroutine(ShowTxtForTime(text));
        }
    }

    private IEnumerator ShowTxtForTime(GameObject txt)
    {
        txt.SetActive(true);
        yield return new WaitForSeconds(showTime);
        txt.SetActive(false);
        _showing = false;
        if (secondText != null && _textsShown == 1)
        {
            _showing = true;
            _textsShown++;
            StartCoroutine(ShowTxtForTime(secondText));
        }
        else _textsShown = 0;
    }
}
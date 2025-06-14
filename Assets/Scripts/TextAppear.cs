using System.Collections;
using TMPro;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject secondText;
    [SerializeField] private float showTime = 5f;
    [SerializeField] private float glowSpeed = 2f;
    
    private bool _showing = false;
    private int _textsShown = 0;

    public void ShowText()
    {
        if (!_showing)
        {
            _showing = true;
            _textsShown++;
            StartCoroutine(ShowTxtForTime(text));
        }
    }

    private IEnumerator ShowTxtForTime(GameObject txt)
    {
        txt.SetActive(true);
        //yield return new WaitForSeconds(showTime);
        
        
        var tmp = txt.GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogWarning("Missing TextMeshProUGUI component!");
            yield break;
        }

        float timer = 0f;
        Color baseColor = tmp.color;

        while (timer < showTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.25f, 0.75f, Mathf.PingPong(Time.time * glowSpeed, 1f));
            tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }

        tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f); // Reset alpha
        
        
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
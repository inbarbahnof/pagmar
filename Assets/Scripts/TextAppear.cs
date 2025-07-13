using System.Collections;
using TMPro;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private float showTime = 5f;
    [SerializeField] private float glowSpeed = 2f;
    
    private bool _showing = false;
    private bool _showTxt = false;

    public void ShowText()
    {
        _showTxt = true;
        if (!_showing)
        {
            _showing = true;
            StartCoroutine(ShowTxtForTime(text));
        }
    }

    private IEnumerator ShowTxtForTime(GameObject txt)
    {
        txt.SetActive(true);
        
        var tmp = txt.GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogWarning("Missing TextMeshProUGUI component!");
            yield break;
        }

        //float timer = 0f;
        Color baseColor = tmp.color;

        //while (timer < showTime)
        while (_showTxt)
        {
            //timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.25f, 0.75f, Mathf.PingPong(Time.time * glowSpeed, 1f));
            tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }
        
        tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f); // Reset alpha
        
        txt.SetActive(false);
        _showing = false;
    }

    public void StopShowText()
    {
        _showTxt = false;
    }

    public void RegisterAsCallListener(InputManager playerInput)
    {
        playerInput.RegisterCallListener(this);
    }
}
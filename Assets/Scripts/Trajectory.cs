using UnityEngine;
using UnityEngine.Serialization;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotsNum;
    [FormerlySerializedAs("ditsParent")] [SerializeField] private GameObject dotsParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing;

    private Transform[] _dotsList;
    private Vector2 _pos;
    private float _timeStamp;

    private void Start()
    {
        Hide();
        PrepareDots();
    }

    public void Show()
    {
        dotsParent.SetActive(true);
    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }

    public void UpdateDots(Vector2 startPoint, Vector2 endPoint, Vector2 controlPoint)
    {
        for (int i = 0; i < dotsNum; i++)
        {
            _dotsList[i].position = Evaluate((float) i / dotsNum, startPoint, endPoint, controlPoint);
        }
    }

    private void PrepareDots()
    {
        _dotsList = new Transform[dotsNum];
        for (int i = 0; i < dotsNum; i++)
        {
            _dotsList[i] = Instantiate(dotPrefab, null).transform;
            _dotsList[i].parent = dotsParent.transform;
        }
    }
    
    private Vector2 Evaluate(float t, Vector2 startPoint, Vector2 endPoint, Vector2 controlPoint)
    {
        Vector2 sc = Vector2.Lerp(startPoint, controlPoint, t);
        Vector2 ce = Vector2.Lerp(controlPoint, endPoint, t);
        return Vector2.Lerp(sc, ce, t);
    }
}
using UnityEngine;
using UnityEngine.Serialization;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotsNum;
    [SerializeField] private GameObject dotsParent;
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
            _dotsList[i].position = TrajectoryEvaluator.Evaluate((float) i / dotsNum, startPoint, endPoint, controlPoint);
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
    
}
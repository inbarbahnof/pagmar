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

    public void UpdateDots(Vector2 objPos, Vector2 force)
    {
        _timeStamp = dotSpacing;
        for (int i = 0; i < dotsNum; i++)
        {
            _pos.x = (objPos.x + force.x * _timeStamp);
            _pos.y = (objPos.y + force.y * _timeStamp) - (Physics2D.gravity.magnitude*_timeStamp*_timeStamp) /2f;

            _dotsList[i].position = _pos;
            _timeStamp += dotSpacing;
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
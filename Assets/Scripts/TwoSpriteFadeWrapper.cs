using UnityEngine;

public class TwoSpriteFadeWrapper : MonoBehaviour
{ 
    [SerializeField] private MonoBehaviour startObjRaw;
    [SerializeField] private MonoBehaviour endObjRaw;

    private IObjectFader startObj;
    private IObjectFader endObj;

    void Awake()
    {
        startObj = startObjRaw.GetComponent<IObjectFader>();
        endObj = endObjRaw.GetComponent<IObjectFader>();

        if (startObj == null || endObj == null)
        {
            Debug.LogError("Assigned objects must implement IObjectFader");
        }
    }

    public void SwapIn(bool reverse)
    { 
        if (startObj == null || endObj == null) return;

        if (reverse)
        {
            endObj.FadeOutOverTime(false);
            startObj.FadeOutOverTime(true);
        }
        else
        {
            startObj.FadeOutOverTime(false);
            endObj.FadeOutOverTime(true);
        }
    }
}
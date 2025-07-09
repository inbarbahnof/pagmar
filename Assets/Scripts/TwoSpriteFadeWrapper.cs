using UnityEngine;

public class TwoSpriteFadeWrapper : MonoBehaviour
{
        [SerializeField] private IObjectFader startObj;
        [SerializeField] private IObjectFader endObj;

        public void SwapIn(bool reverse)
        {
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
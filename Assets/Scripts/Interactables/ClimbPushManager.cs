using UnityEngine;

namespace Interactables
{
    public class ClimbPushManager : MonoBehaviour
    {
        [SerializeField] private GameObject rightCol;
        [SerializeField] private GameObject leftCol;
        [SerializeField] private Collider2D leftClimbTrigger;
        [SerializeField] private Collider2D leftJumpTrigger;
        [SerializeField] private Collider2D rightClimbTrigger;
        [SerializeField] private Collider2D rightJumpTrigger;
        
        public void PlayerOnOffLog(bool onLog)
        {
            leftCol.SetActive(onLog);
            rightCol.SetActive(onLog);
            leftClimbTrigger.enabled = !onLog;
            rightClimbTrigger.enabled = !onLog;
            leftJumpTrigger.enabled = onLog;
            rightJumpTrigger.enabled = onLog;
        }
    }
}
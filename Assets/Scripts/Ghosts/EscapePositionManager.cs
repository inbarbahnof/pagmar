namespace Ghosts
{
    using UnityEngine;

    public class EscapePositionManager : MonoBehaviour
    {
        [SerializeField] private GameObject _escapeGroup;

        private Transform[] _escapePoints;

        public static EscapePositionManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            if (_escapeGroup != null)
            {
                _escapePoints = new Transform[_escapeGroup.transform.childCount];
                for (int i = 0; i < _escapeGroup.transform.childCount; i++)
                {
                    _escapePoints[i] = _escapeGroup.transform.GetChild(i);
                }
            }
            else
            {
                Debug.LogWarning("EscapePositionManager: _escapeGroup not assigned.");
                _escapePoints = new Transform[0];
            }
        }

        public Vector3 GetClosestEscapePoint(Vector3 proposedTarget, Vector3 ghostiePos)
        {
            if (_escapePoints == null || _escapePoints.Length == 0)
                return proposedTarget; // fallback

            Transform closest = _escapePoints[0];
            float minDist = Vector3.Distance(proposedTarget, closest.position);

            foreach (var point in _escapePoints)
            {
                if (Vector3.Distance(point.position, ghostiePos) < 0.5f)
                {
                    // Skip this point – ghostie is already at or too close to it
                    continue;
                }
                
                float dist = Vector3.Distance(proposedTarget, point.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = point;
                }
            }

            return closest.position;
        }
    }
}
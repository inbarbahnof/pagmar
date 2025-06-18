using UnityEngine;
using UnityEngine.VFX;

namespace Ghosts
{
    public class GhostieDie : MonoBehaviour
    {
        [SerializeField] private GameObject _intireGhostie;
        [SerializeField] private GameObject _detecror;
        [SerializeField] private GhostieMovement _movement;
        
        [Header("Die Art")]
        [SerializeField] private Rigidbody2D[] _bones;
        [SerializeField] private VisualEffect[] _vfx;
        
        private AttackDetector _attackDetector;
        private Vector3 _initialPos;
        private Vector3[] _initialBonePositions;
        private Quaternion[] _initialBoneRotations;

        private void Awake()
        {
            _attackDetector = GetComponent<AttackDetector>();
            _initialPos = _intireGhostie.transform.position;
            
            _initialBonePositions = new Vector3[_bones.Length];
            _initialBoneRotations = new Quaternion[_bones.Length];
            for (int i = 0; i < _bones.Length; i++)
            {
                _initialBonePositions[i] = _bones[i].transform.localPosition;
                _initialBoneRotations[i] = _bones[i].transform.localRotation;
            }
        }
        
        public void Die(Vector3 pos, Cage cage)
        {
            _detecror.SetActive(false);
            _movement.Die(pos, cage);
            _attackDetector.IsAttackEnabled(false);
            
            foreach (var bone in _bones)
            {
                bone.bodyType = RigidbodyType2D.Dynamic;
            }
            
            foreach (var effect in _vfx)
            {
                effect.Stop();
            }
        }

        public GameObject GetGhostie()
        {
            return _intireGhostie;
        }

        public void Live()
        {
            _detecror.SetActive(true);
            _movement.ResetMovement();
            _attackDetector.IsAttackEnabled(true);
            
            foreach (var effect in _vfx)
            {
                effect.Play();
            }
            
            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].bodyType = RigidbodyType2D.Kinematic;
                _bones[i].transform.localPosition = _initialBonePositions[i];
                _bones[i].transform.localRotation = _initialBoneRotations[i];
                _bones[i].velocity = Vector2.zero;
                _bones[i].angularVelocity = 0f;
            }
            
            _intireGhostie.transform.position = _initialPos;
        }
    }
}
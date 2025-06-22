using System.Collections;
using Targets;
using UnityEngine;
using UnityEngine.VFX;

namespace Ghosts
{
    public class GhostieDie : MonoBehaviour
    {
        [SerializeField] private GameObject _intireGhostie;
        [SerializeField] private GameObject _detecror;
        [SerializeField] private GhostieMovement _movement;
        [SerializeField] private AttackTarget _target;
        
        [Header("Die Art")]
        [SerializeField] private Rigidbody2D[] _bones;
        [SerializeField] private VisualEffect[] _vfx;
        [SerializeField] private Animator[] _boneAnimators;
        
        private AttackDetector _attackDetector;
        private Vector3 _initialPos;
        private Vector3[] _initialBonePositions;
        private Quaternion[] _initialBoneRotations;
        private SpriteRenderer[] _boneRenderers;
        private int[] _initialSortingOrders;

        private void Awake()
        {
            _attackDetector = GetComponent<AttackDetector>();
            _initialPos = _intireGhostie.transform.position;
            
            _initialBonePositions = new Vector3[_bones.Length];
            _initialBoneRotations = new Quaternion[_bones.Length];
            _boneRenderers = new SpriteRenderer[_bones.Length];
            _initialSortingOrders = new int[_bones.Length];
            
            for (int i = 0; i < _bones.Length; i++)
            {
                _initialBonePositions[i] = _bones[i].transform.localPosition;
                _initialBoneRotations[i] = _bones[i].transform.localRotation;
                _boneRenderers[i] = _bones[i].GetComponent<SpriteRenderer>();
                _initialSortingOrders[i] = _boneRenderers[i].sortingOrder;
            }
        }
        
        public void Die(Vector3 pos, Cage cage)
        {
            _detecror.SetActive(false);
            _movement.Die(pos, cage, this);
            _attackDetector.IsAttackEnabled(false);
            StartCoroutine(WaitToFinishTargetAction());
            
            foreach (var effect in _vfx)
            {
                effect.Stop();
            }
        }

        private IEnumerator WaitToFinishTargetAction()
        {
            yield return new WaitForSeconds(2f);
            _target.FinishTargetAction();
        }

        public void DropBones()
        {
            foreach (var animator in _boneAnimators)
            {
                animator.enabled = false;
            }
            
            foreach (var bone in _bones)
            {
                bone.bodyType = RigidbodyType2D.Dynamic;
            }
            
            foreach (var renderer in _boneRenderers)
            {
                renderer.sortingOrder = 4;
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
                _bones[i].linearVelocity = Vector2.zero;
                _bones[i].angularVelocity = 0f;
                _boneRenderers[i].sortingOrder = _initialSortingOrders[i];
                _boneAnimators[i].enabled = true;
            }
            
            _intireGhostie.transform.position = _initialPos;
        }
    }
}
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Misc.Physic
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private bool _isOverrideMass;
        [SerializeField] private bool _enableOnAwake;
        
        [SerializeField] private float _mass;
        [SerializeField] private Rigidbody[] _rbs;
        [SerializeField] private Rigidbody _centerBody;
        
        [SerializeField] private Collider[] _colliders;

        [SerializeField] private UnityEvent _onEnableRagdoll;
        [SerializeField] private UnityEvent _onDisableRagdoll;

        public Rigidbody CenterBody => _centerBody;

        private List<BonesInfo> _bonesInfo = new List<BonesInfo>();

        public bool IsRagdollActive
        {
            set
            {
                foreach (var rb in _rbs)
                {
                    rb.isKinematic = value == false;
                    
                    if(_isOverrideMass)
                        rb.mass = _mass / _rbs.Length;
                }

                foreach (var collider1 in _colliders)
                {
                    collider1.enabled = value;
                }

                if (value)
                    _onEnableRagdoll.Invoke();
                else
                    _onDisableRagdoll.Invoke();
            }
        }

        private void Awake()
        {
            InitBonesInfo(transform);
            IsRagdollActive = _enableOnAwake;
        }

        public void Reload()
        {
            IsRagdollActive = false;
            ResetBones();
        }

        private void InitBonesInfo(Transform parent)
        {
            _bonesInfo.Add(new BonesInfo
            {
                Bone = parent,
                StartLocalPosition = parent.localPosition,
                StartLocalRotation = parent.localRotation
            });
            foreach (Transform child in parent)
            {
                InitBonesInfo(child);
            }
        }

        public void ResetBones()
        {
            foreach (var bonesInfo in _bonesInfo)
            {
                bonesInfo.Bone.localPosition = bonesInfo.StartLocalPosition;
                bonesInfo.Bone.localRotation = bonesInfo.StartLocalRotation;
            }
        }

        public void AddForce(Vector3 position, Vector3 force, ForceMode mode)
        {
            foreach (var rb in _rbs)
            {
                rb.AddForceAtPosition(force, position, mode);
            }
        }

        [Button]
        private void Setup()
        {
            _rbs = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        [Button()]
        private void ActivateRagdoll()
        {
            IsRagdollActive = true;
        }
        
        [Button()]
        private void DisableRagdoll()
        {
            IsRagdollActive = false;
        }
    }
    
    public struct BonesInfo
    {
        public Transform Bone;
        public Vector3 StartLocalPosition;
        public Quaternion StartLocalRotation;
    }
}
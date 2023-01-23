using System;
using AYellowpaper;
using NaughtyAttributes;
using NavySpade._PJ71.Positions;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Colliders
{
    public abstract class ColliderFitter : MonoBehaviour, IColliderFieldChanger
    {
        [SerializeField] private InterfaceReference<IArea> _area;
        [SerializeField] private Vector3 _colliderPadding = Vector3.up;

        protected IArea Area => _area.Value;

        protected Vector3 Padding => _colliderPadding;

        public event Action FieldsChanged;

        private void Start()
        {
            _area.Value.SizeChanged += UpdateCollider;
            UpdateCollider();
        }

        private void OnDestroy()
        {
            _area.Value.SizeChanged -= UpdateCollider;
        }

        protected void OnColliderFieldChanged()
        {
            FieldsChanged?.Invoke();
        }
        
        protected abstract void UpdateCollider();
    }
}
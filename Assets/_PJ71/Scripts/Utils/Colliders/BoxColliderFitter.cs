using System;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Colliders
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderFitter : ColliderFitter
    {
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void OnValidate()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }
        }

        [Button]
        protected override void UpdateCollider()
        {
            Vector3 areaSize = Area.RectSize;
            _collider.size = new Vector3(areaSize.x + Padding.x, Padding.y, areaSize.y + Padding.z);

            Vector3 center = Area.Center;
            _collider.center = center;

            OnColliderFieldChanged();
        }
    }
}
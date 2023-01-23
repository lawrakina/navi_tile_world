using UnityEngine;

namespace NavySpade._PJ71.Utils.Colliders
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderView : ColliderView
    {
        [SerializeField] private Transform _viewContent;
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private Vector2 _padding;

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

        protected override void UpdateView()
        {
            Vector3 colliderCenter = _collider.center;
            colliderCenter.y = _viewContent.localPosition.y;
            _viewContent.localPosition = colliderCenter;
            
            Vector3 colliderSize = _collider.size;
            foreach (var rend in _renderers)
            {
                rend.size = new Vector2(colliderSize.x + _padding.x, colliderSize.z + _padding.y);
            }
        }
    }
}
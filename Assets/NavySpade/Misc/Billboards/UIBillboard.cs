using UnityEngine;

namespace Misc.Billboards
{
    [RequireComponent(typeof(Canvas))]
    public class UIBillboard : BillboardBase
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _yOffset = 1.5f;

        private Camera _camera;

        private void Awake()
        {
            Init(transform.parent);
            _camera = Camera.main;
            _canvas.worldCamera = _camera;

            transform.SetParent(_camera.transform);
        }

        private void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
                return;
            }

            UpdatePosition();
        }

        private void OnBecameVisible()
        {
            enabled = true;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        private void UpdatePosition()
        {
            var newPosition = new Vector3(Target.position.x, Target.position.y + _yOffset, Target.position.z);
            transform.position = newPosition;
        }

        protected override void LookAtTarget()
        {
            
        }
    }
}
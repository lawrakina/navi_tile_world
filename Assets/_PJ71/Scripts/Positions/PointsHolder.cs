using System;
using NaughtyAttributes;
using NavySpade._PJ71.Positions;
using UnityEngine;

namespace NavySpade._PJ71.Scripts.Positions
{
    public abstract class PointsHolder : MonoBehaviour, IArea
    {
        [SerializeField]
        [Foldout("Debug")]
        private bool _isDebug;
        
        [ReadOnly]
        public Vector3[] _positions;
        
        public abstract Vector2 RectSize { get; }
        
        public abstract Vector3 Center { get; }
        public event Action SizeChanged;

        [Button("Set")]
        private void Awake()
        {
            _positions = GetPositions();
        }

        public Vector3 GetPosition(int index)
        {
            return _positions[index];
        }
        
        protected void OnSizeChanged()
        {
            SizeChanged?.Invoke();
        }

        protected abstract Vector3[] GetPositions();
        
        public abstract Vector3 GetPointRotation();
        
        
        public abstract void ChangeSize(Vector2Int size);


        #region EDITOR

        private void OnDrawGizmos()
        {
            if(_isDebug == false)
                return;
        
            for (int i = 0; i < _positions.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_positions[i], 0.3f);
            
                Gizmos.color = Color.blue;
                if (i < _positions.Length - 1)
                {
                    Gizmos.DrawLine(_positions[i], _positions[i+1]);
                }
            }
        }

        #endregion
    }
}
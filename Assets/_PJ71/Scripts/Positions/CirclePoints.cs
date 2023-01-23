using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavySpade._PJ71.Scripts.Positions
{
    public class CirclePoints : PointsHolder
    {
        [Serializable]
        private struct Ring
        {
            public float Distance;
            public int RingCount;
        }

        [SerializeField] private Ring[] _rings;

        public override Vector2 RectSize => Vector2.zero;
        public override Vector3 Center => transform.position;
        
        protected override Vector3[] GetPositions()
        {
            var list = new List<Vector3>();

            for (int i = 0; i < _rings.Length; i++)
            {
                list.AddRange(GetPositionAround(transform.position, _rings[i].Distance, _rings[i].RingCount));
            }
            
            return list.ToArray();
        }
        
        private Vector3[] GetPositionAround(Vector3 startPos, float distance, int positionsCount)
        {
            var array = new Vector3[positionsCount];

            for (var i = 0; i < positionsCount; i++)
            {
                var angle = i * (360f / positionsCount);
                var direction = Quaternion.Euler(0, angle + 90, 0) * Vector3.forward;
                array[i] = startPos + direction * distance;
            }

            return array;
        }

        public override Vector3 GetPointRotation()
        {
            return Vector3.forward;
        }
        
        public override void ChangeSize(Vector2Int size) { }
        
    }
}
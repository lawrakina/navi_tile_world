using System;
using System.Collections.Generic;
using UnityEngine;

namespace pj33
{
    public class RingSquadPositions : SquadPositions
    {
        [Serializable]
        public struct Ring
        {
            public float Distance;
            public int RingCount;
            public float Offset;
        }

        [SerializeField] private Ring[] _rings;

        public bool IsIncludeCenterPoint;
        
        
        private Vector3[] _localPositions;

        public override Vector3[] LocalPositions => _localPositions;

        //https://youtu.be/mCIkCXz9mxI?t=863

        private void Awake()
        {
            _localPositions = GetPositions();
        }

        private void OnValidate()
        {
            _localPositions = GetPositions();
        }

        private Vector3[] GetPositions()
        {
            var circle = GetPositionAround(Vector3.zero, _rings);

            if (!IsIncludeCenterPoint) 
                return circle;
            
            var array = new Vector3[circle.Length + 1];
                
            array[0] = Vector3.zero;

            for (var i = 1; i < array.Length; i++)
            {
                array[i] = circle[i - 1];
            }

            return array;
        }

        private static Vector3[] GetPositionAround(Vector3 startPosition, Ring[] rings)
        {
            var list = new List<Vector3>();

            for (int i = 0; i < rings.Length; i++)
            {
                list.AddRange(GetPositionAround(startPosition, rings[i].Distance, rings[i].RingCount, rings[i].Offset));
            }

            return list.ToArray();
        }

        private static Vector3[] GetPositionAround(Vector3 startPos, float distance, int positionsCount, float offset)
        {
            var array = new Vector3[positionsCount];

            for (var i = 0; i < positionsCount; i++)
            {
                var angle = i * (360f / positionsCount) + offset;
                var direction = Quaternion.Euler(0, 0, angle + 90) * new Vector3(1, 0);
                array[i] = startPos + direction * distance;

                //hack fot top-down games in x,z
                array[i] = new Vector3(array[i].x, 0, array[i].y);
            }

            return array;
        }
    }
}
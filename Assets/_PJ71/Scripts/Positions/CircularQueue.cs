using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade
{
    public class CircularMovementUnitQueue : MonoBehaviour
    {
        [Serializable]
        public struct Ring
        {
            public float Distance;
            public int RingCount;
        }

        [SerializeField] private Ring[] _rings;

        [Foldout("Debug")] [SerializeField] private bool _isDrawDebug;

        private Vector3[] _positions;
        
        private static Vector3[] GetPositionAround(Vector3 startPosition, Ring[] rings)
        {
            var list = new List<Vector3>();

            for (int i = 0; i < rings.Length; i++)
            {
                list.AddRange(GetPositionAround(startPosition, rings[i].Distance, rings[i].RingCount));
            }

            return list.ToArray();
        }

        private static Vector3[] GetPositionAround(Vector3 startPos, float distance, int positionsCount)
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

        public Vector3 GetPosition(int unitIndex)
        {
            return GetPositionAround(transform.position, _rings)[unitIndex];
        }

        public bool IsEnable => enabled;

        private void OnDrawGizmos()
        {
            if(_isDrawDebug == false)
                return;

            Gizmos.color = Color.red;
            foreach (var vector3 in GetPositionAround(transform.position, _rings))
            {
                Gizmos.DrawSphere(vector3, .3f);
            }
        }
    }
}

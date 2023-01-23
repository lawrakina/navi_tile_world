using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Project_60.Movement
{
    public class Patrolling : PathFinder
    {
        private enum PatrolType
        {
            PingPong,
            Loop,
            Once,
        }
        
        [SerializeField] private Transform[] _points;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private PatrolType _patrolType;

        private int _currentPointIndex;
        private bool _pathIsEnd;
        private bool _isReverse;
        
        private void Start()
        {
            _currentPointIndex = -1;
        }
        
        public void SetPoints(Transform[] points)
        {
            _points = points;
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }
        
        public override bool TryGetNextPoint(out Vector3 nextPoint)
        {
            return GetNextPoint(ref _currentPointIndex, out nextPoint);
        }

        private bool GetNextPoint(ref int currentIndex, out Vector3 nextPoint)
        {
            switch (_patrolType)
            {
                case PatrolType.PingPong:
                    if (currentIndex == 0 || currentIndex == _points.Length - 1)
                    {
                        _isReverse = !_isReverse;
                    }
                    currentIndex += _isReverse ? -1 : 1;
                    break;
                case PatrolType.Once:
                    currentIndex++;
                    if (currentIndex >= _points.Length)
                    {
                        currentIndex = _points.Length - 1;
                        _pathIsEnd = true;
                    }
                    break;
                case PatrolType.Loop:
                    currentIndex++;
                    if (currentIndex > _points.Length - 1)
                    {
                        currentIndex = 0;
                    }
                    break;
            }

            if (_pathIsEnd)
            {
                nextPoint = Vector3.zero;
                return false;
            }
            else
            {
                currentIndex = Mathf.Clamp(currentIndex, 0, _points.Length);
                nextPoint = _points[currentIndex].position + _offset;
                return true;
            }
        }

        [Button]
        private void Setup()
        {
            List<Transform> temp = new List<Transform>();
            foreach (Transform child in transform)
            {
                temp.Add(child);
            }

            _points = temp.ToArray();
        }
    }
}

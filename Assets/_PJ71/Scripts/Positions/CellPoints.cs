using Core.Extensions.UnityTypes;
using NavySpade._PJ71.Scripts.Utils;
using UnityEngine;

namespace NavySpade._PJ71.Scripts.Positions
{
    public class CellPoints : PointsHolder
    {
        [SerializeField] private Vector2 _distanceBetweenPoints;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private AlignmentType _alignmentType;
        [SerializeField] private Vector3 _pointDirection = Vector3.forward;
        
        public override Vector2 RectSize => new Vector2(
            _size.x * _distanceBetweenPoints.x,
            _size.y * _distanceBetweenPoints.y);

        public override Vector3 Center {
            get
            {
                if (_size != Vector2Int.zero)
                {
                    Vector3 first = GetPointLocalPosition(0, 0);
                    Vector3 last = GetPointLocalPosition(_size.x - 1, _size.y - 1);
                    return (first + last) / 2;
                }
                
                return Vector3.zero;
            }
        }

        protected override Vector3[] GetPositions()
        {
            Vector3[] positions = new Vector3[_size.x * _size.y];
            
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    Vector3 pos = transform.TransformPoint(GetPointLocalPosition(x, y));
                    positions[y * _size.x + x] = pos;
                }
            }

            return positions;
        }
        
        private Vector3 GetPointLocalPosition(int x, int y)
        {
            Vector3 offsetForFirstPoint = GridUtils.GetOriginOffset(
                _alignmentType, _size, _distanceBetweenPoints);
            
            Vector3 localPos = new Vector3(_distanceBetweenPoints.x * x, 0, _distanceBetweenPoints.y * y);
            localPos -= offsetForFirstPoint;
            return localPos;
        }
        
        public override void ChangeSize(Vector2Int size)
        {
            _size = size;
            _positions = GetPositions();
            OnSizeChanged();
        }
        
        public override Vector3 GetPointRotation()
        {
            return transform.backward();
        }
    }
}
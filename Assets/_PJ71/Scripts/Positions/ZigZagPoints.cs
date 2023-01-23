using UnityEngine;

namespace NavySpade._PJ71.Scripts.Positions
{
    public class ZigZagPoints : PointsHolder
    {
        [SerializeField] private Vector2 _distanceBetweenPoints;
        [SerializeField] private Vector2Int _size;

        public override Vector2 RectSize => new Vector2(
            _size.x * _distanceBetweenPoints.x,
            _size.y * _distanceBetweenPoints.y);

        public override Vector3 Center { get; }

        private void Awake()
        {
            _positions = GetPositions();
        }

        public override Vector3 GetPointRotation()
        {
            return Vector3.forward;
        }

        public override void ChangeSize(Vector2Int size)
        {
            _size = size;
            _positions = GetPositions();
        }

        protected override Vector3[] GetPositions()
        {
            Vector3[] positions = new Vector3[_size.x * _size.y];

            Vector3 offsetForFirstPoint = new Vector3(
                (_size.x - 1) * _distanceBetweenPoints.x / 2,
                0,
                (_size.y - 1) * _distanceBetweenPoints.y / 2);

            Vector3 startPos = transform.position - new Vector3(offsetForFirstPoint.x, 0, offsetForFirstPoint.z * -1);

            bool reverse = false;
            for (int y = _size.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    int newY = (_size.y - 1 - y);
                    int newX = reverse ? _size.x - 1 - x : x;

                    Vector3 offset = new Vector3(_distanceBetweenPoints.x * x, 0, _distanceBetweenPoints.y * newY * -1);
                    positions[newY * _size.x + newX] = startPos + offset;
                }

                reverse = !reverse;
            }

            return positions;
        }
    }
}
using NavySpade._PJ71.Scripts.Positions;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace NavySpade
{
    public class PointsVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private PointsHolder _pointsHolder;

        private void Start()
        {
            _pointsHolder.SizeChanged += InitPoints;
            InitPoints();
        }

        private void OnDestroy()
        {
            _pointsHolder.SizeChanged -= InitPoints;
        }

        private void InitPoints()
        {
            transform.DestroyChildren();
            foreach (var point in _pointsHolder._positions)
            {
                Vector3 spawnPos = point;
                spawnPos.y = 0.01f;
                Instantiate(_prefab, spawnPos, _prefab.transform.rotation, transform);
            }
        }
    }
}

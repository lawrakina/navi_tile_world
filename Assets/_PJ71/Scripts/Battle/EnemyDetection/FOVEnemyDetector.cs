using NavySpade._PJ71.Utils;
using UnityEngine;

namespace NavySpade._PJ71.Battle.EnemyDetection
{
    public class FOVEnemyDetector : EnemyDetector
    {
        [SerializeField] private FieldOfView _fieldOfView;

        private void Start()
        {
            _fieldOfView.FoundTarget += UpdateVisibleTargets;
        }
        
        public override void Init(float viewRadiusValue)
        {
            _fieldOfView.ViewRadius = viewRadiusValue;
        }

        private void OnDestroy()
        {
            _fieldOfView.FoundTarget -= UpdateVisibleTargets;
        }

        private void UpdateVisibleTargets()
        {
            ClearEnemies();
            
            var visibleTargets = _fieldOfView.VisibleTargets;
            foreach (var visibleTarget in visibleTargets)
            {
                TryAddEnemy(visibleTarget, out _);
            }
        }


    }
}
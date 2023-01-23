using System.Collections.Generic;
using Core.Damagables;

namespace NavySpade._PJ71.Battle.EnemyDetection
{
    public interface IEnemyDetector
    {
        public bool HasTargets { get; }

        public IEnumerable<DamageableMono> Enemies { get; }
    }
}
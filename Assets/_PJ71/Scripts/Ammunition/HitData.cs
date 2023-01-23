using Core.Damagables;
using UnityEngine;

namespace Misc.Damagables.Effects
{
    public class HitData : IDamageParameter
    {
        public Vector3 HitPoint;
        public Vector3 HitNormal;

        public HitData(Vector3 hitPoint, Vector3 hitNormal)
        {
            HitPoint = hitPoint;
            HitNormal = hitNormal;
        }
    }
}
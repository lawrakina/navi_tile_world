using UnityEngine;

namespace Project_60.Movement
{
    public abstract class PathFinder : MonoBehaviour
    {
        public abstract bool TryGetNextPoint(out Vector3 nextPoint);
    }
}

using UnityEngine;

namespace Misc.Billboards
{
    public abstract class BillboardBase : MonoBehaviour
    {
        protected Transform Target { get; private set; }

        public void Init(Transform target)
        {
            Target = target;
        }

        protected abstract void LookAtTarget();
    }
}
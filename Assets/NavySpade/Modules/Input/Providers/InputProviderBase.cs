using Core.Game;
using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace Core.Input.Providers
{
    public abstract class InputProviderBase<T> : MonoBehaviour
    {
        public T Value { get; protected set; }
        
        public bool IsPointerDown { get; protected set; }

        protected abstract void OnPonterDown();

        protected abstract void UpdateValue();

        public virtual void ClearInput()
        {
            Value = default;
            IsPointerDown = false;
        }

        protected bool IsActive()
        {
            return true;
        }
    }
}
using System;
using System.Collections;

namespace Core.Loading.Operations
{
    public class ActionOperation : IAsyncOperation
    {
        private Action _action;

        public ActionOperation(Action action)
        {
            _action = action;
        }
        
        public IEnumerator Load()
        {
            _action.Invoke();
            yield return null;
        }
    }
}
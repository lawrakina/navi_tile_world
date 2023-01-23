using System;
using System.Collections;

namespace Core.Loading.Operations
{
    public class FuncOperation : IAsyncOperation
    {
        private Func<IEnumerator> _func;
        
        public FuncOperation(Func<IEnumerator> func)
        {
            _func = func;
        }
        
        public IEnumerator Load()
        {
            yield return _func.Invoke();
        }
    }
}
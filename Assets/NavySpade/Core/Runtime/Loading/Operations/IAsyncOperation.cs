using System.Collections;

namespace Core.Loading.Operations
{
    public interface IAsyncOperation
    {
        public IEnumerator Load();
    }
}
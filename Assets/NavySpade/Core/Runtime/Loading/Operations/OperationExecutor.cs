using System.Collections;

namespace Core.Loading.Operations
{
    public class OperationExecutor
    {
        public IEnumerator Execute(params IAsyncOperation[] operations)
        {
            foreach (var operation in operations)
            {
                yield return operation.Load();
            }
        }
    }
}
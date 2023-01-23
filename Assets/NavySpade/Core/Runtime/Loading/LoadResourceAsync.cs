using System.Collections;
using Core.Loading.Operations;
using UnityEngine;

namespace Core.Loading
{
    public class LoadResourceAsync<T> : IAsyncOperation where T : Object
    {
        private string _resourcePath;
        
        public T Asset { get; private set; }
        
        public LoadResourceAsync(string path)
        {
            _resourcePath = path;
        }
        
        public IEnumerator Load()
        {
            var request = Resources.LoadAsync<T>(_resourcePath);
            yield return request;
            Asset = request.asset as T;
        }
    }
}
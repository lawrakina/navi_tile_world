using System.Collections.Generic;

namespace NavySpade.pj46.Extensions
{
    public static class CacheMany<T>
    {
        private static List<T> _instances;
        
        public static T Read(int token)
        {
            return _instances[token];
        }

        public static int Bind(T obj, int bufferDefaultSize = 1)
        {
            if (_instances == null)
                _instances = new List<T>(1);
            
            _instances.Add(obj);
            return _instances.Count - 1;
        }

        public static void Remove(int token) => _instances[token] = default;

        public static void Clear() => _instances.Clear();
    }
    
    
    public static class CacheSingle<T>
    {
        private static T _instance;

        public static T Read()
        {
            return _instance;
        }

        public static void Bind(T obj)
        {
            _instance = obj;
        }
        
        public static void Clear() => _instance = default;
    }
}
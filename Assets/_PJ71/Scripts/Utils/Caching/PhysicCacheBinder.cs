using UnityEngine;

namespace NavySpade.pj46.Extensions
{
    public static class PhysicCacheBinder
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Bind()
        {
            CacheSingle<RaycastHit[]>.Bind(new RaycastHit[512]);
            CacheSingle<Collider[]>.Bind(new Collider[50]);
        }

        public static RaycastHit[] Read()
        {
            return CacheSingle<RaycastHit[]>.Read();
        }
    }
}
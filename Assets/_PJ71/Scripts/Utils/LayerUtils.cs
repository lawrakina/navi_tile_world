using UnityEngine;

namespace NS.Core.Utils
{
    public static class LayerUtils
    {
        public static bool CheckForComparerLayer(LayerMask layerMask, GameObject gameObject)
        {
            return (layerMask.value & (1 << gameObject.layer)) != 0;
        }
    }
}
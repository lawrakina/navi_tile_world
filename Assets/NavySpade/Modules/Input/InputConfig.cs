using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Core.Input
{
    public class InputConfig : ObjectConfig<InputConfig>
    {
        [field: Range(0, 8)]
        [field: SerializeField]
        public float Sensetivity { get; private set; }

        [field: SerializeField]
        public ControlType Type { get; private set; }
    }
}
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;


namespace NavySpade._PJ71.InventorySystem {
    public class SquadConfig : ObjectConfig<SquadConfig> {
        [field: SerializeField] public int SquadMaxCount { get; set; } = 5;
    }
}
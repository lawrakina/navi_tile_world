using System;
using Pool;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Items
{
    public class ItemObject : MonoBehaviour
    {
        public ResourcePreset Preset { get; set; }

        public ResourceType Type => Preset.Type;

        public void ReturnToPool()
        {
            if (Preset != null && Preset.FromPool)
            {
                this.Return();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
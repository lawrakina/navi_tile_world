using NaughtyAttributes;
using NavySpade._PJ71.InventorySystem.Items;
using Pool;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Game/PJ71/Resource")]
    public class ResourcePreset : ScriptableObject
    {
        public ResourceType Type;
        
        [ShowAssetPreview]
        public Sprite Icon;
        
        public string DisplayName;
        public ItemObject Prefab;
        
        [Min(1)]
        public int CountPerItem;
        public Vector3 SizeInStack;

        public bool ShowVisual;
        public bool IsCountInInventory;
        public bool FromPool;
        public string PoolName;


        public ItemObject CreateObject(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            ItemObject itemObject;
            if (FromPool)
            {
                itemObject = PoolHandler.Get<ItemObject>(PoolName);
                itemObject.transform.SetParent(parent);
                itemObject.transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                itemObject = Instantiate(Prefab, position, rotation, parent);
            }
            
            itemObject.Preset = this;
            return itemObject;
        }
    }
}
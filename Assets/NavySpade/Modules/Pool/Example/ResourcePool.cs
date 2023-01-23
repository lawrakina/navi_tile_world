using NavySpade._PJ71.InventorySystem.Items;

public class ResourcePool : PoolInGame<ItemObject>
{
    private void Awake()
    {
        Initialize();
    }
}

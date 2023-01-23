using NavySpade._PJ71.InventorySystem.Visualizer;

namespace NavySpade._PJ71.BuildingSystem
{
    public interface IBuildingsHolder
    {
        public UnitInventoryVisualizer UnitInventoryVisualizer { get; }
        public UnitInventoryVisualizer TankInventoryVisualizer { get; }
    }
}
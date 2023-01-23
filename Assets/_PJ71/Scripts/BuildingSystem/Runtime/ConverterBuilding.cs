using System.Linq;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;
using ItemInfo = NavySpade._PJ71.InventorySystem.Items.ItemInfo;

namespace NavySpade._PJ71.BuildingSystem.Runtime.States
{ 
    public class ConverterBuilding : ProductionBuilding
    {
        [SerializeField] private InventoryVisualizer _inputInventory;
        [SerializeField] private ItemSelling _inputZone;
        
        public InventoryVisualizer InputInventory => _inputInventory;
        
        protected override void InitInner()
        {
            ClearNonTargetOutputResources(Info);
            OutputInventory.Init(Info.OutputInventory);
            _inputInventory.Init(Info.InputInventory, ProductionPreset.MaxCapacityOfInputInventory);
            _inputZone.Init(ProductionPreset.ResourceRequirement);
        }
        
        public override bool CanProduce()
        {
            ItemInfo itemInfo = _inputInventory.GetItemInfo(ProductionPreset.ResourceRequirement.Preset.Type);
            if (itemInfo.Amount < ProductionPreset.ResourceRequirement.Value)
                return false;

            return true;
        }

        protected override ResourceType ProductionType { get; set; }

        public override void TakeResourceForProduction()
        {
            InputInventory.Inventory.Reduce(
                ProductionPreset.ResourceRequirement.Preset.Type,
                ProductionPreset.ResourceRequirement.Value);
        }

        public override void CreateItem()
        {
            ItemObject itemObject = ProductionPreset.OutputResource.CreateObject(transform.position, Quaternion.identity);
            OutputInventory.AddItem(itemObject);
            //Debug.Log($"Produced {itemObject}");
        }

        public override object CaptureState()
        {
            Info.InputInventory = InputInventory.Inventory.GetSavingData();
            Info.OutputInventory = OutputInventory.Inventory.GetSavingData();
            return base.CaptureState();
        }

        public override void ReturnResourceForProduction() { }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actors;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Scripts.Positions;
using NS.Core.Actors;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Visualizer
{
    public class UnitInventoryVisualizer : InventoryVisualizer, IActorHandler
    {
        [SerializeField] private PointsHolder _pointsHolder;
        [SerializeField] private Transform _container;

        private readonly List<UnitItem> _units = new List<UnitItem>();
        public int _unitInProduction;
        private bool _initialized;

        public override bool HasFreeSpace => TotalUnits < Inventory.MaxSize;

        public int TotalUnits => Inventory.CurrentCount + _unitInProduction;

        public event Action<UnitItem> UnitAdded; 
        
        public void Init(Vector2Int size)
        {
            _pointsHolder.ChangeSize(size);
            base.Init(size.x * size.y);
        }

        public override Vector3 StackPos => _container.position;

        public override void AddItem(ItemObject itemObject)
        {
            if (Inventory.TryAdd(itemObject.Preset.Type, 1))
            {
                itemObject.transform.parent = null;
                UnitItem unitItem = itemObject as UnitItem;
                AddUnitToZone(unitItem);
                UnitAdded?.Invoke(unitItem);
            }
        }

        public override void AddItemInstant(ResourceType type, int amount) {}

        public override ItemObject PullItem(ResourceType type)
        {
            ItemInfo itemInfo = Inventory.GetItemInfo(type);
            Inventory.Reduce(itemInfo);
            UnitItem itemObject = _units.Last();
            _units.Remove(itemObject);
            return itemObject;
        }

        public UnitItem[] PullAllItem()
        {
            Inventory.Clear();
            var unitItem = _units.ToArray();
            Clear();
            return unitItem;
        }

        public UnitItem[] GetAllItem()
        {
            return _units.ToArray();
        }

        private void AddUnitToZone(UnitItem unitItem)
        {
            MoveUnitToPoint(unitItem, _units.Count);
            _units.Add(unitItem);
        }

        private void MoveUnitToPoint(UnitItem unitItem, int index)
        {
            Vector3 targetPosition = _pointsHolder.GetPosition(index);
            unitItem.MoveTo(targetPosition, _pointsHolder.GetPointRotation());
        }
        
        public void AddUnitToProduction()
        {
            _unitInProduction++;
        }

        public void RemoveUnitFromProduction()
        {
            _unitInProduction = Mathf.Clamp(_unitInProduction - 1, 0, int.MaxValue);
        }

        public void Clear()
        {
            _units.Clear();
            Inventory.Clear();
            _unitInProduction = 0;
        }

        public void EnemyDestroyed(ActorHolder holder)
        {
            for (int i = 0; i < _units.Count; i++)
            {
                UnitItem unitItem = _units[i];
                if (unitItem.ActorHolder == holder)
                {
                    _units.Remove(unitItem);
                    Inventory.Reduce(unitItem.Preset.Type);
                    UpdatePositionsOfUnit();
                    return;
                }
            }
        }

        private void UpdatePositionsOfUnit()
        {
            for (int i = 0; i < _units.Count; i++)
            {
                MoveUnitToPoint(_units[i], i);
            }
        }

        public void InitSavingData(ItemSavingInfo[] infoOutputInventory, Func<ItemObject> createAction)
        {
            if(_initialized)
                return;
            
            if (infoOutputInventory != null && infoOutputInventory.Length > 0)
            {
                ItemSavingInfo savingInfo = infoOutputInventory[0];
                Inventory.AddItem(savingInfo.Type, savingInfo.Amount);
                for (int i = 0; i < savingInfo.Amount; i++)
                {
                    ItemObject itemObject = createAction.Invoke();
                    itemObject.transform.position = _pointsHolder.GetPosition(_units.Count);
                    AddUnitToZone(itemObject as UnitItem);
                }
            }

            _initialized = true;
        }
    }
}
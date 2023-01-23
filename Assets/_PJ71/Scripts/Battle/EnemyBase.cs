using Core.Damagables;
using NaughtyAttributes;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Saving;
using NavySpade._PJ71.Tiles;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public class EnemyBase : Base, ISaveable
    {
        [SerializeField] private GuidSaver _saver;
        
        public override void Init(IBasesHandler basesHandler, ITilesHolder tilesHolder)
        {
            SaveManager.Register(this);
            base.Init(basesHandler, tilesHolder);
            
            RestoreState(null);

            UnitInventoryVisualizer.Inventory.ResourcesCountChanged += OnResourceCountChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(UnitInventoryVisualizer)
                UnitInventoryVisualizer.Inventory.ResourcesCountChanged -= OnResourceCountChanged;
        }

        private void OnResourceCountChanged(ItemInfo item)
        {
            if(CurrentTeam != Team.Player)
                TryStartAttack();
        }

        public object CaptureState()
        {
            SaveManager.Save(_saver.SaveKey, CurrentTeam);
            return null;
        }

        public void RestoreState(object state)
        {
            var team = SaveManager.Load(_saver.SaveKey, CurrentTeam);
            if (team != CurrentTeam)
            {
                OnCaptured(team);
            }
        }

        [Button()]
        public void ForceCapture()
        {
            OnCaptured(Team.Player);
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(_saver.SaveKey);    
        }
    }
}
using System;
using NavySpade._PJ71.Saving;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Items
{
    public class GoldStone : Gold, ISaveable
    {
        [SerializeField] private GuidSaver _saver;
        
        private bool _isPicked;
        
        private void Start()
        {
            SaveManager.Register(this);
            RestoreState(null);
            if (_isPicked)
            {
                Destroy(gameObject);
            }
        }
        
        protected override void Pickup()
        {
            _isPicked = true;
            CaptureState();
            base.Pickup();
        }

        public object CaptureState()
        {
            SaveManager.Save(_saver.SaveKey, _isPicked ? 1 : 0);
            return null;
        }

        public void RestoreState(object state)
        {
            int value = SaveManager.Load(_saver.SaveKey, 0);
            _isPicked = value == 1;
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(_saver.SaveKey);
        }
    }
}
using System;
using AYellowpaper;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEngine;

namespace NavySpade.NavySpade.Modules.Saving.Runtime
{
    public class LocalSaver : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ISaveable> _saveable;
        private bool _isSaved;
        
        private void OnDestroy()
        {
            SaveAll();
        }

        protected void OnApplicationQuit()
        {
            SaveAll();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveAll();

            _isSaved = pause;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(hasFocus == false)
                SaveAll();
            
            _isSaved = hasFocus == false;
        }
        
        private void SaveAll()
        {
            if(_isSaved)
                return;

            _saveable.Value.CaptureState();
            _isSaved = true;
        }
    }
}
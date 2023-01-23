using System;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade._PJ71.Saving
{
    [ExecuteInEditMode]
    public class GuidSaver : MonoBehaviour
    {
        [SerializeField] private string _save;
        [SerializeField] private string _saveGuid;

        public string SaveKey => _save + _saveGuid;
        
        private void Awake()
        {
#if UNITY_EDITOR
            GenerateGuid();
#endif
        }
        
        [Button()]
        public void GenerateGuid()
        {
            if (String.IsNullOrEmpty(_saveGuid))
            {
                _saveGuid = Guid.NewGuid().ToString();
            }
        }
    }
}
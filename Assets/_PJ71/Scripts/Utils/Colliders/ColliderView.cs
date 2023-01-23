using System;
using AYellowpaper;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Colliders
{
    [ExecuteAlways]
    public abstract class ColliderView : MonoBehaviour
    {
        [SerializeField] private bool _updateInRuntime;
        [SerializeField] private bool _updateInEditMode;

        [SerializeField] private bool _hasColliderChanger;
        
        [SerializeField] 
        [ShowIf(nameof(_hasColliderChanger))]
        private InterfaceReference<IColliderFieldChanger> _colliderChanged;

        private void Start()
        {
            if (_hasColliderChanger)
            {
                _colliderChanged.Value.FieldsChanged += UpdateView;
                UpdateView();
            }
        }

        private void OnDestroy()
        {
            if (_hasColliderChanger)
            {
                _colliderChanged.Value.FieldsChanged -= UpdateView;
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                if(_updateInRuntime == false)
                    return;
            }
            else
            {
                if(_updateInEditMode == false)
                    return;
            }

            UpdateView();
        }

        protected abstract void UpdateView();
    }
}
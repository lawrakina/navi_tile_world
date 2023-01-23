using System.Collections.Generic;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Utils;
using UnityEngine;

namespace NavySpade._PJ71.Gathering
{
    public class MinedObjectDetector : MonoBehaviour
    {
        [SerializeField] private FieldOfView _fieldOfView;
        

        private List<MinedObject> _visibleMinedObjects = new List<MinedObject>();
        //private List<MinedObject> _canGatherObjects = new List<MinedObject>();

        public bool CanGather => _visibleMinedObjects.Count > 0;

        public IEnumerable<MinedObject> MinedObjects => _visibleMinedObjects;
        
        private void Start()
        {
            _fieldOfView.FoundTarget += UpdateVisibleTargets;
        }
        
        private void OnDestroy()
        {
            _fieldOfView.FoundTarget -= UpdateVisibleTargets;
        }

        private void UpdateVisibleTargets()
        {
            ClearEnemies();
            
            var visibleTargets = _fieldOfView.VisibleTargets;
            foreach (var visibleTarget in visibleTargets)
            {
                TryAddObject(visibleTarget, out _);
            }
        }
        
        private void ClearEnemies()
        {
            _visibleMinedObjects.Clear();
        }

        private bool TryAddObject(Collider col, out MinedObject result)
        {
            result = null;
            if(col == null)
                return false;

            if (col.TryGetComponent(out MinedObject minedObject))
            {
                minedObject.ResetRecovering();
                if (minedObject.CanGather)
                {
                    result = minedObject;
                    _visibleMinedObjects.Add(minedObject);
                }
                
                
            }

            return true;
        }
    }
}
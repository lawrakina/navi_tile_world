using System;
using System.Collections.Generic;
using Core.Damagables;
using Core.Movement;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Gathering;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Utils;
using pj33;
using UnityEngine;


namespace NavySpade._PJ71.Squad {
    public class SquadDetector : MonoBehaviour {
        [SerializeField] private FieldOfView _fieldOfView;

        private List<UnitItem> _squadUnits = new List<UnitItem>();
        public event Action<UnitItem> ItemAdded;

        public bool CanGather => _squadUnits.Count > 0;

        public List<UnitItem> SquadUnits => _squadUnits;
        

        private void Start() {
            _fieldOfView.FoundTarget += UpdateVisibleTargets;
        }

        private void OnDestroy() {
            _fieldOfView.FoundTarget -= UpdateVisibleTargets;
        }

        private void UpdateVisibleTargets() {
            // ClearEnemies();

            var visibleTargets = _fieldOfView.VisibleTargets;
            foreach (var visibleTarget in visibleTargets)
            {
                TryAddObject(visibleTarget, out _);
            }
        }

        private void ClearEnemies() {
            _squadUnits.Clear();
        }

        private bool TryAddObject(Collider col, out UnitItem result) {
            result = null;
            if (col == null)
                return false;
            if (_squadUnits.Count >= SquadConfig.Instance.SquadMaxCount)
                return false;

            if (col.TryGetComponent(out DontInToSquadFlag _))
            {
                return false;
            }
            if (col.TryGetComponent(out UnitItem item))
            {
                if (item.TeamVisualSwitcher.Team != Team.Player)
                    return false;
                if (!_squadUnits.Contains(item))
                {
                    result = item;
                    _squadUnits.Add(item);
                    ItemAdded?.Invoke(item);
                    item.ActorHolder.Damageable.OnDeath += _ => {
                        _squadUnits.Remove(item);
                    };
                }
            }

            return true;
        }
    }

}
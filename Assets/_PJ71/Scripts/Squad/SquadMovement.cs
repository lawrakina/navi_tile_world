using System;
using System.Collections.Generic;
using System.Linq;
using Core.Movement;
using NavySpade._PJ71.Gathering;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.Squad;
using pj33;
using UnityEngine;


namespace NavySpade._PJ71.Player {
    public class SquadMovement : MonoBehaviour {
        [SerializeField] private RingSquadPositions _ringSquad;
        private List<UnitMovement> _squadUnits = new List<UnitMovement>();
        private SquadDetector _detector;
        public Vector3 SetVelocity {
            set {
                var positions = _ringSquad.LocalPositions;
                for (var index = 0; index < _squadUnits.Count; index++)
                {
                    var unit = _squadUnits[index];
                    unit.Move(positions[index] + transform.position );
                    // unit.MoveTo(positions[index] + transform.position + value);
                }
            }
        }

        public void Init(SquadDetector detector) {
            _detector = detector;
            _detector.ItemAdded += DetectorOnItemAdded;
        }

        private void OnDestroy() {
            _detector.ItemAdded -= DetectorOnItemAdded;
        }

        private void DetectorOnItemAdded(UnitItem item) {
            item.InitMovementOnSquad();
            _squadUnits.Add(item.Movement);// = _detector.SquadUnits.Select(x=>x.Movement).ToList();
        }

        public void Clear() {
            _squadUnits.Clear();
        }
    }
}
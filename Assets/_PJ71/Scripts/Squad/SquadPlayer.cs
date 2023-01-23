using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Player;
using UnityEngine;


namespace NavySpade._PJ71.Squad {
    public class SquadPlayer : MonoBehaviour {
        [field: SerializeField]
        public SquadDetector Detector { get; set; }
        [field: SerializeField]
        public SquadMovement SquadMovement { get; set; }

        private PlayerStackVisualizer _stack;

        public void Init(PlayerStackVisualizer stack) {
            SquadMovement.Init(Detector);
            _stack = stack;
            
            Detector.ItemAdded += DetectorOnItemAdded;
        }

        private void OnDestroy() {
            Detector.ItemAdded -= DetectorOnItemAdded;
        }

        private void DetectorOnItemAdded(UnitItem item) {
            _stack.AddUnitToSquad(item);
            item.InitOnSquad(_stack);
        }

        public void Clear() {
            _stack.ClearSquad();
            SquadMovement.Clear();
        }
    }
}
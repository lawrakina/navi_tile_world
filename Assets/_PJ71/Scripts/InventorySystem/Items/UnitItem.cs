using System.Collections;
using Core.Actors;
using Core.Input.Commands;
using Core.Movement;
using NavySpade._PJ71.Actors.States;
using NavySpade._PJ71.Extensions;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Scripts.Actors.Runtime;
using NavySpade._PJ71.Tiles;
using NavySpade._PJ71.Utils;
using UnityEngine;
using UnityEngine.EventSystems;


namespace NavySpade._PJ71.InventorySystem.Items {
    public class UnitItem : ItemObject {
        [SerializeField] private UnitMovement _unitMovement;
        [SerializeField] private UnitStackVisualizer _unitStackVisualizer;
        [SerializeField] private UnitSquadTileUnlockerTrigger _tileUnlocker;
        [SerializeField] private ShootingUnit _shootingUnit;

        public UnitMovement Movement => _unitMovement;
        public UnitStackVisualizer Inventory => _unitStackVisualizer;
        public TeamVisualSwitcher TeamVisualSwitcher;
        public ActorHolder ActorHolder;
        
        [SerializeField] private bool _removeOnSquad;

        private Vector3 _targetRotation;
        private Vector3 _targetPosition;

        public void MoveTo(Vector3 point, Vector3 pointRotation) {
            _targetPosition = point;
            _targetRotation = pointRotation;
            ReturnToPos();
        }

        public void ReturnToPos() {
            _unitMovement.MoveTo(new MoveCommand(_targetPosition, PointReached));
        }

        private void PointReached() {
            transform.rotation = Quaternion.LookRotation(_targetRotation);
        }

        public void InitOnSquad(PlayerStackVisualizer owner) {
            if(_unitStackVisualizer) _unitStackVisualizer.Init(owner);
            if(_tileUnlocker) _tileUnlocker.Init(owner);
            if(_removeOnSquad)_shootingUnit.HandlerRemoveOn();
            gameObject.tag = StringManager.UNIT_PLAYER_SQUAD;
            ActorHolder.OnDied += UnitDied;
        }

        private void UnitDied(ActorHolder obj) {
            ActorHolder.OnDied -= UnitDied;
            _unitMovement.ToDefault();
            _unitStackVisualizer.ToDefault();
            Inventory.Clear();
        }

        public void InitMovementOnSquad() {
            _unitMovement.Init(stopingdistance: 0.04f, acceleration: 2.5f);
        }
    }
}
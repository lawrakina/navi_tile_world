using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actors;
using Core.Damagables;
using Core.Input.Commands;
using NaughtyAttributes;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade._PJ71.InventorySystem.Visualizer;
using NavySpade._PJ71.Scripts.Actors.Runtime;
using NavySpade._PJ71.Tiles;
using NavySpade.NavySpade.Modules.Damageble.Damagables.Teams;
using NS.Core.Actors;
using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public class Base : MonoBehaviour, IBuildingsHolder, IActorHandler, ITeam
    {
        [SerializeField] private Team _team;
        [SerializeField] private UnitInventoryVisualizer _unitInventoryVisualizer;
        [SerializeField] private UnitInventoryVisualizer _tankInventoryVisualizer;
        [SerializeField] private Vector2Int _size = new Vector2Int(3, 3);
        [SerializeField] private Vector2Int _tankSize = Vector2Int.one;
        [SerializeField] private Flag _flag;
        [SerializeField] private Transform _unitContainer;

        private IBasesHandler _basesHandler;
        private ITilesHolder _tilesHolder;

        private Tile[] _tiles;
        private BuildingHandler[] _buildings;
        private List<ShootingUnit> _shootingUnits;

        private bool _isBlock;
        
        public Flag Flag => _flag;

        public Team CurrentTeam
        {
            get => _team;
            set => _team = value; 
        }

        public UnitInventoryVisualizer UnitInventoryVisualizer => _unitInventoryVisualizer;
        public UnitInventoryVisualizer TankInventoryVisualizer => _tankInventoryVisualizer;

        public event Action<Base> BaseIsCaptured;
        
        public event Action<Team> TeamChanged;

        public virtual void Init(IBasesHandler basesHandler, ITilesHolder tilesHolder)
        {
            if (_unitInventoryVisualizer)
            {
                _unitInventoryVisualizer.Init(_size);
                _unitInventoryVisualizer.UnitAdded += CheckUnitTarget;
            }

            if (_tankInventoryVisualizer)
            {
                _tankInventoryVisualizer.Init(_tankSize);
                _tankInventoryVisualizer.UnitAdded += CheckUnitTarget;
            }

            _basesHandler = basesHandler;
            _tilesHolder = tilesHolder;
            
            _flag.Init(_team);
            _flag.IsCaptured += OnCaptured;
            _flag.StateChanged += OnFlagStateChanged;

            _tiles = _tilesHolder.GetTiles((t) => t.Team == _team);
            _buildings = _tilesHolder.GetContent<BuildingHandler>((b) => b.Team == _team);
            Array.ForEach(_buildings, (b) => b.Init(this));

            //units is not from barracks
            if (_unitContainer != null)
            {
                _shootingUnits = _unitContainer.GetComponentsInChildren<ShootingUnit>().ToList();
                _shootingUnits.ForEach((b) => b.Init(this, b.Preset, _team));
            }
        }

        protected virtual void OnDestroy()
        {
            if (_unitInventoryVisualizer)
            {
                _unitInventoryVisualizer.UnitAdded -= CheckUnitTarget;
            }

            if (_tankInventoryVisualizer)
            {
                _tankInventoryVisualizer.UnitAdded -= CheckUnitTarget;
            }

            _flag.IsCaptured -= OnCaptured;
            _flag.StateChanged -= OnFlagStateChanged;
        }

        public void TryStartAttack()
        {
            if(_isBlock)
                return;
            
            if (_basesHandler.TryGetTargetFlag(_flag, out Flag destinationFlag))
            {
                if(CanAttack() == false)
                    return;
                StartAttack(destinationFlag);
            }
        }

        [Button("ForceStartAttack")]
        public void ForceStartAttack() {
            _basesHandler.TryGetTargetFlag(_flag, out Flag destinationFlag);
            StartAttack(destinationFlag);
        }

        private void StartAttack(Flag destinationFlag ) {
            if (_unitInventoryVisualizer)
            {
                UnitItem[] units = _unitInventoryVisualizer.PullAllItem();
                Array.ForEach(units, (u) => { ((ShootingUnit) u.ActorHolder).MoveToFlag(destinationFlag); });
                _unitInventoryVisualizer.Clear();
            }

            if (_tankInventoryVisualizer)
            {
                UnitItem[] tanks = _tankInventoryVisualizer.PullAllItem();
                Array.ForEach(tanks, (u) => { ((ShootingUnit) u.ActorHolder).MoveToFlag(destinationFlag); });
                _tankInventoryVisualizer.Clear();
            }
        }

        private bool CanAttack()
        {
            if (CurrentTeam == Team.Player)
            {
                return true;
            }
            else
            {
                return _unitInventoryVisualizer.HasFreeSpace == false;  
            }
        }

        protected virtual void OnCaptured(Team team)
        {
            _team = team;
            _flag.SwitchTeam(team);

            var teamData = TilemapConfig.Instance.GetTeamData(team);
            Array.ForEach(_tiles, (t) => t.UpdateTeamData(teamData));
            Array.ForEach(_buildings, (b) => b.SetTeam(team));
            Array.ForEach(_unitInventoryVisualizer.GetAllItem(), (u) => u.ActorHolder.Damageable.CurrentTeam = team);
            if (_tankInventoryVisualizer)
            {
                Array.ForEach(_tankInventoryVisualizer.GetAllItem(),
                              (u) => u.ActorHolder.Damageable.CurrentTeam = team);
            }

            _shootingUnits.ForEach((b) => b.Damageable.CurrentTeam = team);
            
            BaseIsCaptured?.Invoke(this);
            TeamChanged?.Invoke(CurrentTeam);
        }

        public void EnemyDestroyed(ActorHolder holder)
        {
            _shootingUnits.Remove((ShootingUnit) holder);
        }

        public void BlockAttack(bool isBlock)
        {
            _isBlock = isBlock;
            if (_isBlock == false)
            {
                TryStartAttack();
            }
        }
        
        private void OnFlagStateChanged(CaptureState state)
        {
            Array.ForEach(_unitInventoryVisualizer.GetAllItem(), CheckUnitTarget);
        }
        
        private void CheckUnitTarget(UnitItem unitItem)
        {
            ShootingUnit unit;
            switch (_flag.CurrentCaptureState)
            {
                case CaptureState.Capturing: 
                    unit = ((ShootingUnit) unitItem.ActorHolder);
                    unit.MoveToFlag(_flag);
                    unit.CanAttack = false;
                    break;
                case CaptureState.Draw:
                    unit = ((ShootingUnit) unitItem.ActorHolder);
                    unit.MoveToFlag(_flag);
                    unit.CanAttack = true;
                    break;
                case CaptureState.FullControl:
                    unitItem.ReturnToPos();
                    break;
            }
        }
    }
}
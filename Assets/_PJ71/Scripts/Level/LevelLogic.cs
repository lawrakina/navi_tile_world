using System;
using System.Collections;
using Core.Damagables;
using Main.Levels.Data;
using NavySpade._PJ71.Battle;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Tiles;
using NavySpade.Core.Runtime.Levels;
using NavySpade.NavySpade.Modules.Utils.Timers;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace NavySpade._PJ71.Level
{
    public class LevelLogic : LevelBase, IBasesHandler
    {
        [SerializeField] private PlayerHandler _playerHandler;
        [SerializeField] private TileMapHandler _tileMapHandler;
        [SerializeField] private float _delayForBakeMesh = 1f;
        
        [SerializeField] private Base[] _bases;
        [SerializeField] private NevMeshBaker _navMeshBaker;
        [SerializeField] private LevelLogicGameEvent _levelLoadedEvent;
        [SerializeField] private UnityEvent _onLevelLoaded;
        [SerializeField] private OpeningCutScene _openingCutScene;

        private TilemapBuildingPlace[] _buildingPlaces;
        private NavMeshPath _path;
        private BattleFieldConfig _battleFieldConfig;
        private Timer _timerForBakeMesh;
        private bool _meshBaked;

        public PlayerHandler Player => _playerHandler;
        
        public bool CanAttack { get; set; }

        public override void Init(LevelDataBase data)
        {
            _battleFieldConfig = BattleFieldConfig.Instance;
            
            _tileMapHandler.Init();
            _playerHandler.Init();
            _path = new NavMeshPath();
            _timerForBakeMesh = new Timer(_delayForBakeMesh);

            _navMeshBaker.Bake();
            Array.ForEach(_bases, (b) =>
            {
                b.Init(this, _tileMapHandler);
                b.BaseIsCaptured += CheckLevelCondition;
            });
            
            _tileMapHandler.TileUnlocked += StartDelayForUpdateNavMesh;

            _onLevelLoaded?.Invoke();
            _levelLoadedEvent.Raise(this);
            
            TryStartAttackPlayer();
            SetTargetFlagCutScene();
        }
        
        private void OnDestroy()
        {
            _tileMapHandler.TileUnlocked -= StartDelayForUpdateNavMesh;
        }

        public IEnumerator WaitForBakeMesh()
        {
            while (_navMeshBaker.HasMesh == false)
            {
                yield return null;
            }
        }

        private void Update()
        {
            if(_timerForBakeMesh == null)
                return;

            if (_meshBaked == false)
            {
                BakeMesh();
                _meshBaked = true;
                _timerForBakeMesh.Update(Time.deltaTime);
                if (_timerForBakeMesh.IsFinish())
                {
                    
                }
            }
        }

        private void StartDelayForUpdateNavMesh()
        {
            _timerForBakeMesh.initTime = _delayForBakeMesh;
            _timerForBakeMesh.Reload();
            _meshBaked = false;
        }

        private void BakeMesh()
        {
            _navMeshBaker.Bake();
            TryStartAttackPlayer();
            CheckAttackEnemyState();
        }
        
        private void TryStartAttackPlayer()
        {
            foreach (var b in _bases)
            {
                if(b.Flag.CurrentTeam == Team.Player)
                    continue;
                
                b.TryStartAttack();
            }
        }
        
        private void CheckAttackEnemyState()
        {
            CanAttack = false;
            foreach (var b in _bases)
            {
                if(b.Flag.CurrentTeam == Team.Player)
                    continue;

                CanAttack |= TryGetTargetFlag(b.Flag, out _);
            }
        }
        
        public void TryStartAttackEnemy()
        {
            foreach (var b in _bases)
            {
                if (b.Flag.CurrentTeam == Team.Player)
                {
                    b.TryStartAttack();
                }
            }
        }
        
        public bool TryGetTargetFlag(Flag flag, out Flag destinationFlag)
        {
            var minDistance = int.MaxValue;
            destinationFlag = null;
            
            foreach (var b in _bases)
            {
                if (_battleFieldConfig.CanAttack(b.Flag.CurrentTeam, flag.CurrentTeam))
                {
                    
                    NavMesh.CalculatePath(
                        b.Flag.transform.position, 
                        flag.transform.position, NavMesh.AllAreas, _path);

                    bool hasPath = _path.status == NavMeshPathStatus.PathComplete;

                    var distance = Vector3.Distance(b.Flag.transform.position, flag.transform.position);
                    if (hasPath && distance < minDistance)
                    {
                        destinationFlag = b.Flag;
                    }
                }
            }
            
            return destinationFlag != null;
        }
        
        private void CheckLevelCondition(Base unitBase)
        {
            bool hasPlayerBase = false;
            bool hasEnemyBase = false;
            Base enemyBase = null;
            
            foreach (var b in _bases)
            {
                hasPlayerBase |= b.Flag.CurrentTeam == Team.Player;
                
                bool playerCanAttack = _battleFieldConfig.CanAttack(Team.Player, b.Flag.CurrentTeam);
                hasEnemyBase |= playerCanAttack;
                if (playerCanAttack)
                {
                    enemyBase = b;
                }
            }

            if (hasEnemyBase == false)
            {
                InvokeWinState();
            }
            else if (hasPlayerBase == false)
            {
                InvokeLoseState();
            }
            else
            {
                if(unitBase.Flag.CurrentTeam == Team.Player)
                    SetTargetFlagCutScene();
            }
        }

        private void SetTargetFlagCutScene()
        {
            foreach (var b in _bases)
            {
                // if(b.Flag.OwnerFlagTeam == Team.Player)
                //     continue;

                if (b.Flag.CurrentTeam != Team.Player)
                {
                    _openingCutScene.SetTCameraFollow(b.Flag.transform);
                    _openingCutScene.PlayCutScene();
                    return;
                }
            }
        }
    }
}

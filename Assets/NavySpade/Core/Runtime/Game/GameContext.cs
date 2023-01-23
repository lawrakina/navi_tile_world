using Core.Game;
using Core.Meta;
using NavySpade._PJ71.InventorySystem;
using NavySpade._PJ71.InventorySystem.Items;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Saving.Runtime;
using Pj_61_Weapon_adv.Common;
using pj40.Core.Tweens.Runtime;
using UnityEngine;


namespace NavySpade.Core.Runtime.Game{
    public class GameContext : MonoBehaviour
    {
        public static GameContext Instance { get; private set; }
        
        //[field: SerializeField] public ParticleManager ParticleManager { get; private set; }
        
        [field: SerializeField] public InGameEarnedCurrency EarnedCurrency { get; private set; }
        
        [field: SerializeField] public GameStatesManager GameStatesManager { get; private set; }
        
        [field: SerializeField] public LevelManagerBase LevelManager { get; private set; }

        [field: SerializeField] public ItemReceiveAnimation ReceiveItemAnimation { get; private set; }
        
        [field: SerializeField] public GoldSpawner GoldSpawner { get; private set; }
        
        public void Init()
        {
            Instance = this;
            
            //ParticleManager.Init();
            LevelManager.Init();
            GameStatesManager.Init(LevelManager, EarnedCurrency);
            
            ReceiveItemAnimation.Speed = ItemManagementConfig.Instance.ReceivingResourceSpeed;
        }

        public void ResetData()
        {
            SaveManager.ClearAllRegistered();
            MetaGameConfig.Instance.ClearUpgradeProgress();
            
            ReceiveItemAnimation.ClearAllTweens();
            GoldSpawner.ReturnToPool();
            
            //EarnedCurrency.ObservedCurrency.Reset();
        }
    }
}
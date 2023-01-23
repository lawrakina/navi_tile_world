using System.Collections;
using Core.Meta;
using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using NavySpade._PJ71.Meta;
using NavySpade.Core.Runtime.Game;
using NavySpade.Meta.Runtime.Shop.Items;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.UI.Popups.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Popups
{
    public class ItemUnlockingPopup : Popup
    {
        private const string PREFS_LAST_EARN_COUNT_KEY = "Meta.Chest.LastEarned";
        
        [SerializeField] protected Image _unlockIcon;
        [SerializeField] protected TextMeshProUGUI _itemName;

        [Header("HasProgress")] 
        [SerializeField] private bool _hasProgress;
        [SerializeField] [ShowIf(nameof(_hasProgress))] protected Image _lockedIcon; 
        [SerializeField] [ShowIf(nameof(_hasProgress))] protected TextMeshProUGUI _progressText;
        [SerializeField] [ShowIf(nameof(_hasProgress))] private float _fillSpeed;

        public override void OnAwake() { }

        public override void OnStart()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            var shopItems = MetaGameConfig.Instance.Rewards;
            int reachedLvl = GameContext.Instance.LevelManager.LevelIndex + 1;

            for (int i = 0; i < shopItems.Length; i++)
            {
                ShopItem shopItem = shopItems[i];
                LevelUnlockCondition levelUnlockCondition = shopItem.UnlockConditions[0] as LevelUnlockCondition;
                if (levelUnlockCondition.Level <= reachedLvl)
                {
                    ItemReward itemReward = shopItem.Product.Reward as ItemReward;
                    _unlockIcon.sprite = itemReward.Data.Value.ChestUnlockIcon;
                    _itemName.text = itemReward.Data.Value.Name;
                    
                    if (_hasProgress)
                    {
                        _lockedIcon.sprite = itemReward.Data.Value.ChestLockedIcon;

                        int lastUnlockLvl = GetPreviousChestUnlockedLvl(i);
                        float currentValue = Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.Level, reachedLvl);
                        float previousValue = Mathf.InverseLerp(lastUnlockLvl, levelUnlockCondition.Level, reachedLvl - 1);
            
                        StartCoroutine(UpdateProgress(previousValue, currentValue));
                    }
                }
            }
        }
        
        private IEnumerator UpdateProgress(float previousValue, float currentValue)
        {
            _unlockIcon.fillAmount = previousValue;
            
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * _fillSpeed;
                float newValue = Mathf.Lerp(previousValue, currentValue, progress);
                _unlockIcon.fillAmount = newValue;
                _progressText.text = ((int)(newValue * 100)) + "%";
                yield return null;
            }
        }
        
        private int GetPreviousChestUnlockedLvl(int index)
        {
            if (index == 0)
                return 1;

            ShopItem currentUnlockingItem = MetaGameConfig.Instance.Rewards[index - 1];
            LevelUnlockCondition levelUnlockCondition = currentUnlockingItem.UnlockConditions[0] as LevelUnlockCondition;
            return levelUnlockCondition.Level;
        }
        
        public void NextLevel()
        {
            GlobalParameters.DoubleLevelNumber++;
            Close();
            
            EventManager.Invoke(GameStatesEM.NextLevel);
        }
    }
}
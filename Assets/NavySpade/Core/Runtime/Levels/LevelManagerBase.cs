using System;
using Main.Levels.Configuration;
using Main.Levels.Data;
using NaughtyAttributes;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels
{
    public abstract class LevelManagerBase : MonoBehaviour, ILevelManager
    {
        private const string LevelIndexSaveKey = "LevelIndexSaveKey";
        
        private LevelsConfig _levelsConfig;
        
        private int? _levelIndex;

        public int LevelIndex
        {
            get
            {
                _levelIndex ??= SaveManager.Load<int>(LevelIndexSaveKey);
                return (int)_levelIndex;
            }
            set
            {
                _levelIndex = value;
                LevelIndexChanged?.Invoke(value);
                SaveManager.Save(LevelIndexSaveKey, value);
            }
        }
        
        public int LastOpenedLevelIndex
        {
            get => SaveManager.Load<int>("LastOpenedLevelIndex");
            set => SaveManager.Save("LastOpenedLevelIndex", value);
        }
        
        public bool IsTutorialLevel => LevelIndex < _levelsConfig.Tutorial.Length;
        
        public bool IsNextLevel { get; set; }
        
        public LevelBase CurrentLevel { get; set; }
        
        public event Action<int> LevelIndexChanged;
        
        public void Init()
        {
            _levelsConfig = LevelsConfig.Instance;
        }
        
        [Button]
        public void LoadPreviousLevel()
        {
            LevelIndex = ClampLevelIndex(LevelIndex - 1);
            LoadLevel(LevelIndex);
        }

        [Button]
        public void RestartLevel()
        {
            LoadLevel(LevelIndex);
        }
        
        public void UnlockNextLevel()
        {
            LevelIndex = ClampLevelIndex(LevelIndex + 1);
            IsNextLevel = true;
        }
        
        [Button]
        public void LoadNextLevel()
        {
            LevelIndex = ClampLevelIndex(LevelIndex + 1);
            LoadLevel(LevelIndex);
        }
        
        public abstract void LoadLevel(int levelIndex);

        private int ClampLevelIndex(int levelIndex)
        {
            levelIndex = Mathf.Max(levelIndex, 0);
            int realLevelIndex = levelIndex - _levelsConfig.Tutorial.Length;
            LastOpenedLevelIndex = Mathf.Max(LastOpenedLevelIndex, levelIndex);
            
            if (realLevelIndex >= _levelsConfig.Main.Length)
            {
                levelIndex = _levelsConfig.Tutorial.Length;
            }

            return levelIndex;
        }
        
        protected LevelDataBase GetLevelData(int levelIndex)
        {
            if (levelIndex < 0)
                throw new ArithmeticException();

            if (levelIndex < _levelsConfig.Tutorial.Length)
                return _levelsConfig.Tutorial[levelIndex];
            
            var deltaLevel = levelIndex - _levelsConfig.Tutorial.Length;
            deltaLevel = (int)Mathf.Repeat(deltaLevel, _levelsConfig.Main.Length);
            return _levelsConfig.Main[deltaLevel];
        }
    }
}
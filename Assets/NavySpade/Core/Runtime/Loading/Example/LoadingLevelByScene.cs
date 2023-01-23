using System.Collections;
using Core.Extensions;
using Core.Loading;
using Core.Loading.Operations;
using Core.Meta;
using EventSystem.Runtime.Core.Managers;
using Main.Levels.Data;
using NavySpade._PJ71.Level;
using NavySpade.Core.Runtime.Game;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine.SceneManagement;

namespace Pj_61_Weapon_adv.Common.Loading
{
    public class LoadingLevelByScene : IAsyncOperation
    {
        private const string LEVEL_NAME = "Level";

        private SceneLevelData _levelData;
        private SceneLevelManager _sceneLevelManager;

        public LoadingLevelByScene(SceneLevelManager sceneLevelManager, SceneLevelData levelData)
        {
            _levelData = levelData;
            _sceneLevelManager = sceneLevelManager;
        }

        public IEnumerator Load()
        {
            bool needLoad = true;
            if (SceneExtensions.HasSceneBeginningWith(LEVEL_NAME, out var levelScene))
            {
                if (_sceneLevelManager.CurrentLevel != null)
                {
                    if (_sceneLevelManager.IsNextLevel)
                    {
                        _sceneLevelManager.IsNextLevel = false;
                    }
                    
                    yield return SceneManager.UnloadSceneAsync(levelScene);
                }
                else
                {
                    needLoad = false;
                }
            }
            
            if (needLoad)
            {
                var loadingSceneOperation = new LoadingScene(_levelData.BuildIndex);
                yield return loadingSceneOperation.Load();
                levelScene = loadingSceneOperation.LoadedScene;
            }
            
            SceneManager.SetActiveScene(levelScene);
            
            LevelLogic level = levelScene.GetRoot<LevelLogic>();
            _sceneLevelManager.CurrentLevel = level;
            level.Init(_levelData);

            yield return level.WaitForBakeMesh();
            
            EventManager.Invoke(GameStatesEM.LevelLoaded);
        }
    }
}
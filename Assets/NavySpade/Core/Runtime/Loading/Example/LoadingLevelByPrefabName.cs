using System.Collections;
using Core.Loading.Operations;
using EventSystem.Runtime.Core.Managers;
using Main.Levels;
using Main.Levels.Data;
using NavySpade.Core.Runtime.Levels;

namespace Core.Loading.Example
{
    //TODO: Using old levelManager

    public class LoadingLevelByPrefabName : IAsyncOperation
    {
        private const string ASSET_DIR = "Levels/";

        private ResourceNameLevelData _levelData;
        private PrefabLevelManager _prefabLevelManager;

        public LoadingLevelByPrefabName(PrefabLevelManager prefabLevelManager, ResourceNameLevelData levelData)
        {
            _prefabLevelManager = prefabLevelManager;
            _levelData = levelData;
        }

        public IEnumerator Load()
        {
            _prefabLevelManager.DestroyPreviousLevel();

            var request = new LoadResourceAsync<LevelBase>(ASSET_DIR + _levelData.PrefabName);
            yield return request.Load();
            
            _prefabLevelManager.CreateLevel(_levelData, request.Asset);
            
            EventManager.Invoke(GameStatesEM.LevelLoaded);
        }
    }
}
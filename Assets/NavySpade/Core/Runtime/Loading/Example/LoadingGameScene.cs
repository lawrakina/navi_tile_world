using System.Collections;
using Core.Extensions;
using Core.Loading.Operations;
using Core.UI.Popups;
using Main.Levels.Configuration;
using NavySpade.Core.Runtime.App;
using NavySpade.Core.Runtime.Game;
using NavySpade.Modules.Configuration.Runtime.SO;


namespace Core.Loading.Example
{
    public class LoadingGameScene : IAsyncOperation
    {
        public IEnumerator Load()
        {
            LevelsConfig gameConfig = ObjectConfig.GetConfig<LevelsConfig>();
            var loadingSceneOperation = new LoadingScene(gameConfig.GameSceneName);
            yield return loadingSceneOperation.Load();
            
            GameEnterPoint gameEnterPoint = SceneExtensions.GetRoot<GameEnterPoint>(loadingSceneOperation.LoadedScene);
            gameEnterPoint.Init();
        }
    }
}
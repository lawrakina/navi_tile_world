using System.Collections;
using Core.Loading.Operations;
using Main.Levels.Configuration;
using NavySpade.Core.Runtime.App;
using NavySpade.Modules.Configuration.Runtime.SO;


namespace Core.Loading.Example{
    public class LoadingUiScene : IAsyncOperation
    {
        public IEnumerator Load()
        {
            LevelsConfig gameConfig = ObjectConfig.GetConfig<LevelsConfig>();
            var loadingSceneOperation = new LoadingScene(gameConfig.UIScene);
            yield return loadingSceneOperation.Load();
        }
    }
}
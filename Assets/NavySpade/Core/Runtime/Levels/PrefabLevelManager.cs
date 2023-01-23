using Core.Loading.Example;
using Main.Levels.Data;
using NavySpade.Core.Runtime.Game;

namespace NavySpade.Core.Runtime.Levels
{
    public class PrefabLevelManager : LevelManagerBase
    {
        public override void LoadLevel(int levelIndex)
        {
            LevelDataBase levelDataBase = GetLevelData(LevelIndex);

            var loadingOperation = new LoadingLevelByPrefabName(this, levelDataBase as ResourceNameLevelData);
            ProjectContext.Instance.SplashScreen.Execute(loadingOperation);
        }

        public void DestroyPreviousLevel()
        {
            if (CurrentLevel != null)
            {
                Destroy(CurrentLevel.gameObject);
            }
        }

        public void CreateLevel(ResourceNameLevelData data, LevelBase prefab)
        {
            CurrentLevel = Instantiate(prefab);
            CurrentLevel.Init(data);
        }
    }
}
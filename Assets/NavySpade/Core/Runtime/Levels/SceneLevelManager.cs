using EventSystem.Runtime.Core.Managers;
using Main.Levels.Data;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Game;
using Pj_61_Weapon_adv.Common.Loading;

namespace NavySpade.Core.Runtime.Levels
{
    public class SceneLevelManager : LevelManagerBase
    {
        public override void LoadLevel(int levelIndex)
        {
            LevelDataBase levelDataBase = GetLevelData(levelIndex);

            var loadingOperation = new LoadingLevelByScene(this, levelDataBase as SceneLevelData);
            ProjectContext.Instance.SplashScreen.Execute(loadingOperation);
        }
    }
}
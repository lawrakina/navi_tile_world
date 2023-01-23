using Main.Levels.Data;
using UnityEngine;

namespace Main.Levels.Generators
{
    public class SimpleLevelGenerator : LevelGenerator<PrefabLevelData>
    {
        private GameObject _level;

        protected override void OnGenerated(PrefabLevelData dataBase)
        {
            _level = Instantiate(dataBase.Prefab);
        }

        protected override void OnCleanUp()
        {
            Destroy(_level);
        }
    }
}
using Main.Levels.Data;
using UnityEngine;

namespace Main.Levels.Generators
{
    public abstract class LevelGeneratorBase : MonoBehaviour
    {
        public abstract void Generate(LevelDataBase data);
        
        public abstract void CleanUp();
    }
}
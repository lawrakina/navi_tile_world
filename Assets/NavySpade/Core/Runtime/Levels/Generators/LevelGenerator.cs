using System;
using Main.Levels.Data;

namespace Main.Levels.Generators
{
    public abstract class LevelGenerator<T> : LevelGeneratorBase where T : LevelDataBase
    {
        private LevelDataBase.AdditionData[] _datas;

        public override void Generate(LevelDataBase dataBase)
        {
            if (dataBase is T == false)
            {
                throw new Exception("этот уровень имеет другой тип");
            }
            
            OnGenerated(dataBase as T);
            
            if (_datas == null)
            {
                _datas = dataBase.AdditionsData;
                
                foreach (var data in _datas)
                {
                    data.ExtensionData.Apply();
                }
            }
            else
            {
                foreach (var data in _datas)
                {
                    data.ExtensionData.Clear();
                }

                _datas = dataBase.AdditionsData;

                foreach (var data in _datas)
                {
                    data.ExtensionData.Apply();
                }
            }
        }

        public override void CleanUp()
        {
            OnCleanUp();
        }

        protected abstract void OnGenerated(T dataBase);
        
        protected abstract void OnCleanUp();
    }
}
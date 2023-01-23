using EventSystem.Runtime.Core.Managers;
using Main.Levels.Data;
using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Examples
{
    public class ExampleLevel : LevelBase
    {
        public override void Init(LevelDataBase data)
        {
            EventManager.Add(GameStatesEM.StartGame, StartGame);
            
            //GameContext.Instance.LevelManager.UnlockNextLevel();
            //GameContext.Instance.LevelManager.RestartLevel();
        }

        private void StartGame()
        {
            Debug.Log($"Start game?");
        }
    }
}
using UnityEngine;

namespace NavySpade.Core.Runtime.Game
{
    public class GameEnterPoint : MonoBehaviour
    {
        [SerializeField] private GameContext _gameContext;
        
        public void Init()
        {
            _gameContext.Init();
            GameContext.Instance.LevelManager.RestartLevel();
            
            //Todo Insert your Game Logic
        }
    }
}
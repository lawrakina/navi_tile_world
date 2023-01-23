namespace NavySpade.Core.Runtime.Levels
{
    public interface ILevelManager
    {
        public void LoadLevel(int levelIndex);

        public void UnlockNextLevel();
    }
}
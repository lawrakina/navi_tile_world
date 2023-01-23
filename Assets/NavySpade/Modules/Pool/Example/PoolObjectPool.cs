namespace NS.Core.Utils.Pool.Example
{
    public class PoolObjectPool : PoolInGame<PoolObject>
    {
        private void Awake()
        {
            Initialize();
        }
    }
}
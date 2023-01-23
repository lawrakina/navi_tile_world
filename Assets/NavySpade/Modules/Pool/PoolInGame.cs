using LP.ComponentStorage;
using NavySpade.Modules.Pooling.Runtime;
using NS.Core.Utils.Pool;
using Pool;
using UnityEngine;

public abstract class PoolInGame<T> : PoolBehaviorBase where T : Component
{
    [SerializeField] private ObjectPoolComponents<T> _currentPoolComponents;
 
    [Header("Prefab")]
    [SerializeField] private T pooledObject;
    
    /// <summary>
    /// Initialize and preload pool
    /// </summary>
    public virtual void Initialize()
    {
        //Debug.LogError("Initialize");
        _currentPoolComponents = pooledObject.AddPool<T>(PoolKey, InitializeCount, PoolParent);
        
    }

    /// <summary>
    /// get an object from the pool 
    /// </summary>
    /// <returns>T Object </returns>
    public virtual T GetObject()
    {
        var tValue = _currentPoolComponents.Get();
        if (needPoolBeacon)
        {
            if (tValue.TryGetComponent(out PoolBeacon beacon) == false)
            {
                beacon = tValue.gameObject.AddComponent<PoolBeacon>();
            }

            var beaconToInit = new BaseBeacon<T>();
            beacon.ThisBeacon = beaconToInit;
            beaconToInit.Initialize(this, tValue, beacon);
        }

        if (needSaveCS)
        {
#if CStorage
            ComponentStorage.Add(tValue);
#endif
        }
        return tValue;
    }

    /// <summary>
    /// return object to the Pool
    /// </summary>
    /// <param name="returnValue">T Object</param>
    public virtual void Return(T returnValue)
    {
        _currentPoolComponents.Return(returnValue);
    }

    public void ReturnAll()
    {
        _currentPoolComponents.ReturnInGame();
    }
}

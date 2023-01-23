using System;
using UnityEngine;

namespace NS.Core.Utils.Pool
{
    public class PoolObject : MonoBehaviour
    {
        public bool InPoolOnDisable { get; set; }
        
        public void ReturnToPool()
        {
            this.ReturnToThePool();
        }
    }
}
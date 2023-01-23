﻿using System;
using System.Collections.Generic;
using NavySpade.Modules.Pooling.Runtime;
using UnityEngine;

namespace Pool
{
    public static class PoolHandler
    {
        private static Dictionary<string, BasePool> pools = new Dictionary<string, BasePool>();
        private static Dictionary<int, BasePool> inGame= new Dictionary<int, BasePool>();

        public static ObjectPoolComponents<T> AddPool<T>(this T pooledObject, string poolKey, int count, Transform parent) where T : Component
        {
            if (pools.ContainsKey(poolKey))
            {
                throw new Exception("ERROR, KEY SETTED, POOLED CREATE BREAK");
            }

            var pool = new ObjectPoolComponents<T>();
            pool.BaseType = typeof(T);
            pool.Initialize(parent,count,pooledObject);
            pools.Add(poolKey, pool);
            return pool;
        }

        public static T Get<T>(string Key) where T : Component
        {
            if (!pools.ContainsKey(Key))
            {
                throw new Exception("Pool key not found");
            }

            var pool = pools[Key];
            var convertedPool = pool as ObjectPoolComponents<T>;
            var value = convertedPool.Get();
        
            inGame.Add(value.gameObject.GetInstanceID(), pool);
            return value;
        }

        public static void Return<T>(string key, T element) where T: Component
        {
            if (pools.TryGetValue(key, out var value))
            {
                var currentValue = value as ObjectPoolComponents<T>;
                inGame.Remove(element.gameObject.GetInstanceID());
                currentValue.Return(element);
            }

            var pooler = pools[key];
        }
        public static void Return<T>(this T value) where T: Component
        {
            var pooler = inGame[value.gameObject.GetInstanceID()];
            var convertedPool = pooler as ObjectPoolComponents<T>;
            inGame.Remove(value.gameObject.GetInstanceID());
            convertedPool.Return(value);
        }
    }

}


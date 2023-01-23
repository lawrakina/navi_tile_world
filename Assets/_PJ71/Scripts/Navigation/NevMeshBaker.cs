using System;
using NaughtyAttributes;
using NavySpade._PJ71.Battle;
using UnityEngine;
using UnityEngine.AI;

namespace NavySpade
{
    public class NevMeshBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurfaces;
        public bool HasMesh => _navMeshSurfaces.navMeshData != null;

        [Button()]
        public void Bake()
        {
            _navMeshSurfaces.BuildNavMesh();
        }
        
        [Button()]
        public void UpdateNavMesh()
        {
            if (_navMeshSurfaces.navMeshData == null)
            {
                Debug.Log("Build NAVMESH");
                _navMeshSurfaces.BuildNavMesh();
            }
            else
            {
                Debug.Log("UPDATE NAVMESH");
                _navMeshSurfaces.UpdateNavMesh(_navMeshSurfaces.navMeshData);
            }
        }
    }
}

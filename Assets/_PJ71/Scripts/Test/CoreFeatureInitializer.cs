using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade._PJ71.BuildingSystem;
using NavySpade._PJ71.InventorySystem.Visualizer;
using UnityEngine;

namespace NavySpade
{
    public class CoreFeatureInitializer : MonoBehaviour, IBuildingsHolder
    {
        [SerializeField] private PlayerHandler _playerHandler;
        [SerializeField] private UnitInventoryVisualizer _unitInventoryVisualizer;
        [SerializeField] private UnitInventoryVisualizer _tankInventoryVisualizer;

        public UnitInventoryVisualizer UnitInventoryVisualizer => _unitInventoryVisualizer;
        public UnitInventoryVisualizer TankInventoryVisualizer => _tankInventoryVisualizer;

        private void Start()
        {
            _playerHandler.Init();
            _unitInventoryVisualizer.Init(new Vector2Int(3, 3));
            var buildingPlaces = FindObjectsOfType<BuildingPlace>();
            //Array.ForEach(buildingPlaces, (b) => b.Init(this));
        }
    }
}

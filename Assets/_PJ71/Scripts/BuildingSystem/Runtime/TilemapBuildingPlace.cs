using System;
using NavySpade._PJ71.Tiles;
using UnityEngine;

namespace NavySpade._PJ71.BuildingSystem.Runtime
{
    public class TilemapBuildingPlace : BuildingPlace
    {
        private ITilesHolder _tilesHolder;
        
        public void Init(IBuildingsHolder buildingsHolder, ITilesHolder tilesHolder)
        {
            base.Init(buildingsHolder);
            _tilesHolder = tilesHolder;
        }
    }
}
using System;
using Core.Damagables;
using NavySpade._PJ71.BuildingSystem.Runtime;
using NavySpade._PJ71.Tiles;

namespace NavySpade._PJ71.Battle
{
    public class PlayerBase : Base
    {
        private TilemapBuildingPlace[] _buildingPlaces;
        
        public override void Init(IBasesHandler basesHandler, ITilesHolder tilesHolder)
        {
            base.Init(basesHandler, tilesHolder);
            
            _buildingPlaces = tilesHolder.GetContent<TilemapBuildingPlace>(null);
            Array.ForEach(_buildingPlaces, (b) => b.Init(this, tilesHolder));
        }

        protected override void OnCaptured(Team team)
        {
            base.OnCaptured(team);
            Array.ForEach(_buildingPlaces, (b) =>
            {
                if (b.Building != null)
                {
                    b.Building.SetTeam(team);
                }
            });
        }
    }
}
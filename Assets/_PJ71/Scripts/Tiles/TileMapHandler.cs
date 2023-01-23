using System;
using System.Collections.Generic;
using System.Linq;
using Core.Damagables;
using NaughtyAttributes;
using NavySpade._PJ71.Saving;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.Modules.Saving.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NavySpade._PJ71.Tiles
{
    public class TileMapHandler : MonoBehaviour, ITilesHolder, ISaveable
    {
        [SerializeField] private Tilemap _tilesTilemap;
        [SerializeField] private Tilemap _contentTilemap;
        [SerializeField] private Tile[] _tiles;

        private TilemapConfig _tilemapConfig;
        
        private const string SaveKey = "TilemapSave";
        private TilemapSaving _tilemapSaving;
        
        public event Action TileUnlocked;
        
        public void Init()
        {
            _tilemapConfig = TilemapConfig.Instance;
            
            SaveManager.Register(this);
            RestoreState(null);
            
            SetupTiles();
            SetupTileContent();
        }

        private void OnDestroy()
        {
            SaveManager.UnRegister(this);
        }

        private void SetupTiles()
        {
            _tiles = _tilesTilemap.GetComponentsInChildren<Tile>();
            Array.ForEach(_tiles, (t) => t.Init(this));
            
            _tilemapSaving.OpenedTiles.ForEach((s) =>
            {
                Tile tile = TileFactory.GetTile(_tilesTilemap, s);
                if(tile == null)
                    return;
                
                tile.TrySetState(Tile.TileState.Open);
                tile.UpdateTeamData(TilemapConfig.Instance.GetTeamData(Team.Player));
            });
            
            Array.ForEach(_tiles, (t) =>
            {
                if (t.CurrentState == Tile.TileState.Open)
                {
                    SetStateForNeighboringTiles(t, Tile.TileState.CanUnlock);
                    t.UpdateBorders();
                }
            });
        }
        
        private void SetupTileContent()
        {
            var contents = _contentTilemap.GetComponentsInChildren<TileContent>();
            Array.ForEach(contents, (t) => t.Init(this));
        }

        public Tile[] GetOccupiedTiles(Vector3 position, Vector3 size)
        {
            List<Tile> result = new List<Tile>();
            Bounds bounds = new Bounds(position, size);

            foreach (var tile in _tiles)
            {
                if (bounds.Contains(tile.transform.position))
                {
                    result.Add(tile);
                }
            }

            return result.ToArray();
        }

        public void SetStateForNeighboringTiles(Tile tile, Tile.TileState state)
        {
            Array.ForEach(GetNeighboringTiles(tile), (t) => t.TrySetState(state));
        }

        public Tile[] GetNeighboringTiles(Tile tile, bool withCorners = false)
        {
            List<Tile> tiles = new List<Tile>();
            AddTile(Side.Top);
            AddTile(Side.Right);
            AddTile(Side.Bottom);
            AddTile(Side.Left);

            if (withCorners)
            {
                AddTile(Side.LeftTop);
                AddTile(Side.LeftBottom);
                AddTile(Side.RightTop);
                AddTile(Side.RightBottom);
            }

            return tiles.ToArray();

            void AddTile(Side side)
            {
                Tile neighboringTile = TileFactory.GetTile(_tilesTilemap, tile.Pos, side);
                if (neighboringTile != null)
                {
                    tiles.Add(neighboringTile);
                }
            }
        }

        public Tile GetNeighboringTile(Tile tile, Side side)
        {
            return TileFactory.GetTile(_tilesTilemap, tile.Pos, side);
        }
        
        public void OpenNeighboringTiles(Tile tile)
        {
            Tile[] neighboringTiles = GetNeighboringTiles(tile, true);
            Array.ForEach(neighboringTiles, (t) =>
            {
                t.UnlockTile();
            });
        }

        public void OnUnlockTile(Tile tile)
        {
            _tilemapSaving.OpenedTiles.Add(tile.Pos);
            TileUnlocked?.Invoke();
        }
        
        public T[] GetContent<T>(Func<T, bool> predicate) where T : Component
        {
            T[] components = _contentTilemap.GetComponentsInChildren<T>();
            if (predicate == null)
            {
                return components;
            }

            List<T> result = new List<T>();
            Array.ForEach(components, (c) =>
            {
                if (predicate.Invoke(c))
                {
                    result.Add(c);
                }
            });

            return result.ToArray();
        }

        public Tile[] GetTiles(Func<Tile, bool> func)
        {
            return _tiles.Where(func).ToArray();
        }

        [Button()]
        public void RefreshAllTiles()
        {
            _tilesTilemap.RefreshAllTiles();
        }

        #region Saving
        
        public object CaptureState()
        {
            SaveManager.Save(SaveKey, _tilemapSaving);
            return null;
        }

        public void RestoreState(object state)
        {
            _tilemapSaving = SaveManager.Load(SaveKey, new TilemapSaving());
            if (_tilemapSaving.OpenedTiles == null)
            {
                _tilemapSaving.OpenedTiles = new List<Vector2Int>();
            }
        }

        public void ClearSave()
        {
            SaveManager.DeleteKey(SaveKey);
        }
        

        #endregion

        
    }
}
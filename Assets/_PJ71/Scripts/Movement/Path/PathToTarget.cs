using NaughtyAttributes;
using NavySpade.Core.Runtime.Player.Logic;
using UnityEngine;

namespace Project_60.Movement
{
    public class PathToTarget : PathFinder
    {
        public enum SearchType
        {
            Player,
        }
        
        [SerializeField] private bool _isSearchTarget;
        
        [SerializeField] 
        [HideIf(nameof(_isSearchTarget))]
        private Transform _target;

        [SerializeField] 
        [ShowIf(nameof(_isSearchTarget))]
        private SearchType _searchBy;
        
        public override bool TryGetNextPoint(out Vector3 nextPoint)
        {
            if (_isSearchTarget)
            {
                switch (_searchBy)
                {
                    case SearchType.Player:
                        var player = SinglePlayer.Instance;
                        _target = player == null ? null: player.transform;
                        break;
                }
            }
            
            if (_target == null)
            {
                nextPoint = Vector3.zero;
                return false;
            }
            
            nextPoint = _target.position;
            return true;
        }
    }
}

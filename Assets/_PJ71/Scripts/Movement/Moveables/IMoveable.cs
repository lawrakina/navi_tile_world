using System;
using UnityEngine;

namespace NavySpade._PJ71.Movement.Moveables
{
    public interface IMoveable
    {
        public bool IsDestinationReach { get; }
        
        public void Init(MoveSettings moveSettings);
        
        public void Update();
        
        public void MoveTo(Vector3 position, Action destinationReachedCallback = null);
        
        void StopMove();
        
        void StartMove();
    }
}
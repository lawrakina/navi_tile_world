using System;
using NavySpade._PJ71.Movement;
using UnityEngine;

namespace Core.Movement
{
    public abstract class MovementBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected MoveSettings Settings;
        
        public float Speed
        {
            get => Settings.Speed;
            set => Settings.Speed = value;
        }

        public float AngularSpeed
        {
            get => Settings.AngularSpeed;
            set => Settings.AngularSpeed = value;
        }

        public abstract void StartMove();

        public abstract void StopMove();

        public abstract void MoveTo(Vector3 targetPosition, Action onReachCallback = null);
    }
}
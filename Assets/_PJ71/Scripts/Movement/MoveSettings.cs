using System;

namespace NavySpade._PJ71.Movement
{
    [Serializable]
    public struct MoveSettings
    {
        public float Speed;
        public float AngularSpeed;
        public bool UpdateRotation;
        public float StoppingDistance;
    }
}
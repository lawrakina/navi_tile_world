using UnityEngine;

namespace pj40.Core.Tweens
{
    public class ParabolaMovement : MovementTypeBase
    {
        public ParabolaMovement(float arcHeight = 1f)
        {
            ArcHeight = arcHeight;
        }

        public float ArcHeight
        {
            get => _arcHeight;
            set
            {
                _arcHeight = value;

                OnInit();
            }
        }

        private float _targetArcHeight;
        private float _arcHeight;

        public override Vector3 NextPosition(Vector3 targetPosition, float delta)
        {
            // Increment our progress from 0 at the start, to 1 when we arrive.
            Progress = Mathf.Min(Progress + delta * StepScale, 1.0f);

            // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
            float parabola = 1.0f - 4.0f * (Progress - 0.5f) * (Progress - 0.5f);

            // Travel in a straight line from our start position to the target.        
            var nextPosition = Vector3.Lerp(InitialPosition, targetPosition, Progress);

            // Then add a vertical arc in excess of this.
            nextPosition.y += parabola * _targetArcHeight;
            return nextPosition;
        }

        public override void OnInit()
        {
            _targetArcHeight = ArcHeight * Distance;
        }
    }
}
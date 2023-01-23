using UnityEngine;

namespace pj40.Core.Tweens
{
    public class BezierMovement : MovementTypeBase
    {
        private Vector3 _p1;
        private Vector3 _p2;
        private Vector3 _p3;
        private Vector3 _p4;

        private Vector3 _p2OffsetFromP1;
        private Vector3 _p3OffsetFromP4;

        public BezierMovement(Vector3 p2OffsetFromP1, Vector3 p3OffsetFromP4)
        {
            _p2OffsetFromP1 = p2OffsetFromP1;
            _p3OffsetFromP4 = p3OffsetFromP4;
        }
        
        public override void OnInit()
        {
            _p1 = InitialPosition;
            _p2 = _p1 + _p2OffsetFromP1;
        }

        public override Vector3 NextPosition(Vector3 targetPosition, float delta)
        {
            _p4 = targetPosition;
            _p3 = _p4 + _p3OffsetFromP4;
            
            Progress = Mathf.Min(Progress + delta * StepScale, 1.0f);
            
            return Mathf.Pow(1 - Progress, 3) * _p1 + 3 * 
                Mathf.Pow(1 - Progress, 2) * Progress * _p2 + 3 * (1 - Progress) * 
                Mathf.Pow(Progress, 2) * _p3 + Mathf.Pow(Progress, 3) * _p4;
        }
    }
}
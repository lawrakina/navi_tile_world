using Core.Input.Commands.Interfaces;

namespace Core.Input.Commands
{
    public class RotateCommand : ICommand
    {
        //private Rotator _rotator;
        private float _angle;

        public RotateCommand(float angle)
        {
            //_rotator = rotator;
            _angle = angle;
        }

        public void Execute()
        {
            Execute(_angle);
        }

        public void Execute(float angle)
        {
            //if (_rotator.enabled == false)
            //    return;

            //_rotator.UpdateTargetAngle(angle);
        }
    }
}
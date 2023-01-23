using System;

namespace NavySpade.pj77.Tutorial.Condition
{
    public interface ITutorCondition
    {
        public event Action ConditionChanged;

        public void Enable();
        
        public void Disable();
    }
}
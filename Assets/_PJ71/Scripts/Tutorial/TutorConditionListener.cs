using NavySpade.Modules.Utils.Serialization.SerializeReferenceExtensions.Runtime.Obsolete.SR;
using NavySpade.pj77.Tutorial.Condition;
using UnityEngine;

namespace NavySpade.pj77.Tutorial
{
    public class TutorConditionListener : MonoBehaviour
    {
        [SerializeField] private TutorAction _tutorAction;
        [SR, SerializeReference] private ITutorCondition _condition;

        private void OnEnable()
        {
            _condition.Enable();
            _condition.ConditionChanged += InvokeTutor;
        }
        
        private void OnDisable()
        {
            _condition.Disable();
            _condition.ConditionChanged -= InvokeTutor;
        }

        private void InvokeTutor()
        {
            TutorialController.InvokeAction(_tutorAction);
        }
    }
}
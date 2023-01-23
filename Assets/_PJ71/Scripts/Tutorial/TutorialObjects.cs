using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj77.Tutorial
{
    public class TutorialObjects : ExtendedMonoBehavior<TutorialObjects>
    {
        [SerializeField] private TutorAction _tutorAction;
        [SerializeField] private UnityEvent _onShow;
        [SerializeField] private UnityEvent _onHide;

        private void Start()
        {
            _onHide.Invoke();
        }

        public TutorAction TutorAction => _tutorAction;
        
        public static void Show()
        {
            foreach (var tutorObject in All)
            {
                if (tutorObject.TutorAction == TutorialController.Instance.CurrentTutorAction)
                {
                    tutorObject._onShow?.Invoke();      
                }
                else
                {
                    tutorObject._onHide?.Invoke();   
                }
            }
        }

        public static void Hide()
        {
            foreach (var tutorClick in All)
            {
                tutorClick._onHide?.Invoke();
            }
        }
    }
}
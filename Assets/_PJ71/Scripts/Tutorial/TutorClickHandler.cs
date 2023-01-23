using System;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NavySpade.pj77.Tutorial
{
    public class TutorClickHandler : ExtendedMonoBehavior<TutorClickHandler>, IPointerDownHandler
    {
        [SerializeField] private TutorAction _tutorAction;
        [SerializeField] private UnityEvent _onFocus;
        [SerializeField] private UnityEvent _onUnfocus;
        [SerializeField] private UnityEvent _onHide;

        public TutorAction TutorAction => _tutorAction;
        
        public static void Show()
        {
            foreach (var tutorClick in All)
            {
                if (tutorClick.TutorAction == TutorialController.Instance.CurrentTutorAction)
                {
                    tutorClick._onFocus?.Invoke();
                }
                else
                {
                    tutorClick._onUnfocus?.Invoke();
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
        
        public void OnPointerDown(PointerEventData eventData)
        {
            TutorialController.InvokeAction(_tutorAction);
        }
    }
}
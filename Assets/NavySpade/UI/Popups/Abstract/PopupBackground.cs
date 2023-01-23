using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UI.Popups
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupBackground : MonoBehaviour
    {
        [Serializable]
        public class CallbackEvents
        {
            [field: SerializeField] public UnityEvent OnOpen { get; private set; }
            [field: SerializeField] public UnityEvent OnClose { get; private set; }
        }

        [field: SerializeField] public Button Button { get; private set; }

        [field: SerializeField] public CallbackEvents Callbacks { get; private set; }

        private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup =>
            _canvasGroup == null ? _canvasGroup = GetComponent<CanvasGroup>() : _canvasGroup;

        public void OnOpen()
        {
            Callbacks.OnOpen.Invoke();
            //panelImage.DOFade(0.3f, 0.2f);
        }

        public void OnClose()
        {
            //CanvasGroup.DOFade(0.0f, 0.15f).OnComplete(() => { Destroy(gameObject); });
            Callbacks.OnClose.Invoke();
        }
    }
}
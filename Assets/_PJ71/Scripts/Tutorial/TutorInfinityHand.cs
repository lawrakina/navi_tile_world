using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;

namespace NavySpade._PJ71.Tutorial
{
    public class TutorInfinityHand : MonoBehaviour
    {
        [SerializeField] private float _inactiveTime;
        [SerializeField] private GameObject _infiniteHand;

        private Timer _timer;
        
        public bool CanShow { get; set; }

        private void Start()
        {
            _timer = new Timer(_inactiveTime);
        }

        private void Update()
        {
            _timer.Update(Time.deltaTime);
            if (_timer.IsFinish() && CanShow)
            {
                ShowHand();
            }

            if (Input.GetMouseButton(0))
            {
                _timer.Reload();
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                HideHand();
                _timer.Reload();
            }
        }

        private void HideHand()
        {
            _infiniteHand.gameObject.SetActive(false);
        }

        private void ShowHand()
        {
            _infiniteHand.gameObject.SetActive(true);
        }

        private void CanShowHand(bool canShow)
        {
            CanShow = canShow;
        }
    }
}
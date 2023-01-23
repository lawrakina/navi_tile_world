using AYellowpaper;
using NavySpade._PJ71.Battle;
using TMPro;
using UnityEngine;

namespace NavySpade
{
    public class CaptureProgressView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _progressText;
        [SerializeField] private InterfaceReference<ICapturable> _capturable;
        [SerializeField] private bool _hideWhenZero;
        
        private void Start()
        {
            _capturable.Value.CaptureProgressChanged += UpdateView;
            UpdateView(_capturable.Value.CaptureProgress);
        }

        private void OnDestroy()
        {
            _capturable.Value.CaptureProgressChanged -= UpdateView;
        }

        private void UpdateView(float value)
        {
            int percent = ((int) (value * 100));

            if (_hideWhenZero)
            {
                _progressText.gameObject.SetActive(percent > 0);
            }
            
            _progressText.text = percent + "%";
        }
    }
}

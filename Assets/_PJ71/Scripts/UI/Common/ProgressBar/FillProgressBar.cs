using UnityEngine;
using UnityEngine.UI;

namespace NavySpade._PJ71.UI.Common.ProgressBar
{
    public class FillProgressBar : ProgressBarBase
    {
        [SerializeField] private Image _fill;
        [SerializeField] private bool _hideWhenFull;
        [SerializeField] private bool _hideWhenEmpty;

        public override void UpdateProgressBar(float value)
        {
            if (MaxValue == 0)
            {
                if (_hideWhenEmpty)
                    gameObject.SetActive(false);

                return;
            }

            CurValue = value;
            _fill.fillAmount = CurValue / MaxValue;
            gameObject.SetActive(_fill.fillAmount > 0);

            if (CurValue == 0 && _hideWhenEmpty ||
                CurValue >= MaxValue && _hideWhenFull)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
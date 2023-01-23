using UnityEngine;

namespace NavySpade._PJ71.UI.Common.ProgressBar
{
    public class ScaleProgressBar : ProgressBarBase
    {
        [SerializeField] private Transform _fill;
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
            Vector3 scale = _fill.localScale;
            scale.x = CurValue / MaxValue;
            _fill.localScale = scale;
            
            gameObject.SetActive(scale.x > 0);

            if (CurValue <= 0 && _hideWhenEmpty ||
                CurValue >= MaxValue && _hideWhenFull)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
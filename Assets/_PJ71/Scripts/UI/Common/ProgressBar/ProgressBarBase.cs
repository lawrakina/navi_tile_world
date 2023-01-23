using UnityEngine;

namespace NavySpade._PJ71.UI.Common.ProgressBar
{
    public abstract class ProgressBarBase : MonoBehaviour
    {
        public float MaxValue { get; protected set; }
        public float CurValue { get; protected set; }
        
        public void SetupProgressbar(float max, float cur)
        {
            MaxValue = max;
            UpdateProgressBar(cur);
        }

        public abstract void UpdateProgressBar(float value);
    }
}
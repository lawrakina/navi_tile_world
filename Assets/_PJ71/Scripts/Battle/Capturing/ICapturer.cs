using Core.Damagables;
using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public interface ICapturer
    {
        public Team Team { get; }
        
        public void StartCapture(ICapturable capturable);
        
        public void StopCapture(ICapturable capturable);
    }
}
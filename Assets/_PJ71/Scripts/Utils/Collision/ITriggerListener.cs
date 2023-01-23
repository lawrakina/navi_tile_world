using AYellowpaper;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Collision
{
    public interface ITriggerListener
    {
        void DoTriggerEnter(Collider other);
        
        void DoTriggerExit(Collider other);
    }
}
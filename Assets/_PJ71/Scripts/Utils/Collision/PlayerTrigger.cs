using AYellowpaper;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Collision
{
    public class PlayerTrigger : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITriggerListener> _triggerListener;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _triggerListener.Value.DoTriggerEnter(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _triggerListener.Value.DoTriggerExit(other);
            }
        }
    }
}
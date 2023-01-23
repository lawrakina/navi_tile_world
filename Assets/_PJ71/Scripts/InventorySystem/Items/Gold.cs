using NavySpade.Core.Runtime.Game;
using UnityEngine;

namespace NavySpade._PJ71.InventorySystem.Items
{
    public class Gold : MonoBehaviour
    {
        [field: SerializeField] public int Amount { get; private set; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") == false)
                return;
            
            Pickup();
        }

        protected virtual void Pickup()
        {
            GameContext.Instance.GoldSpawner.Receive(this);
        }
    }
}
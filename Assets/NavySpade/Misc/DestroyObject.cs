using UnityEngine;

namespace p21.Entities
{
    public class DestroyObject : MonoBehaviour
    {
        public bool DestroySelfObject = true;
        public GameObject Target;
        
        public void Invoke()
        {
            Destroy(DestroySelfObject ? gameObject : Target);
        }
    }
}
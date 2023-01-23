using UnityEngine;

namespace NavySpade._PJ71.Tutorial
{
    public class TutorSetPosition : MonoBehaviour
    {
        public Transform Target { get; set; }

        public void SetPosition(Transform target)
        {
            Target = target;
        }
        
        private void Update()
        {
            if(Target == null)
                return;

            transform.position = Target.position;
        }
    }
}
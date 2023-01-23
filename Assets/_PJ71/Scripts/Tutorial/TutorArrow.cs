using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace NavySpade.pj77.Tutorial{
    public class TutorArrow : MonoBehaviour
    {
        public Transform Target { get; set; }

        public void SetArrowTarget(Transform target)
        {
            Target = target;
        }
        
        private void Update()
        {
            if(Target == null)
                return;

            Vector3 dir = Target.position - transform.position;
            dir.y = 0;
            
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
using UnityEngine;

namespace NavySpade.pj77.Tutorial
{
    [RequireComponent(typeof(Animation))]
    public class TutorPlayAnimation : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Animation>().Play();
        }
    }
}
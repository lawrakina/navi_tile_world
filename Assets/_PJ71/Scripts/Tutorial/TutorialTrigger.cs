using UnityEngine;

namespace NavySpade.pj77.Tutorial
{
    [RequireComponent(typeof(Collider))]
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private TutorAction _tutorAction;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TutorialController.InvokeAction(_tutorAction);
            }
        }
    }
}
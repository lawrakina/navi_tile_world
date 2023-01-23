using UnityEngine;

namespace NavySpade._PJ71.Battle
{
    public class FightTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                Gui.Instance.FightButton.Show();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                Gui.Instance.FightButton.Hide();
            }
        }
    }
}
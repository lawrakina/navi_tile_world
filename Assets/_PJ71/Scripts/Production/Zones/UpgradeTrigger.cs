using EventSystem.Runtime.Core.Managers;
using NavySpade._PJ71.BuildingSystem;
using UnityEngine;

namespace NavySpade._PJ71.Scripts.Production.Zones
{
    public class UpgradeTrigger : MonoBehaviour
    {
        [SerializeField] private BuildingHandler _building;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                Gui.Instance.UpgradePanel.Show(_building);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                Gui.Instance.UpgradePanel.Hide();
            }
        }
    }
}
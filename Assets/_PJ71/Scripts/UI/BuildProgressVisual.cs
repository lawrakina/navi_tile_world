using NavySpade._PJ71.BuildingSystem;
using TMPro;
using UnityEngine;

namespace NavySpade._PJ71.UI
{
    public class BuildProgressVisual : MonoBehaviour
    {
        [SerializeField] private BuildingPlace _buildingPlace;
        [SerializeField] private TextMeshPro _progressText;
        
        private void Init()
        {
            
        }
        
        private void OnEnable()
        {
            //_buildingPlace.ProgressUpdated += UpdateVisual;
            UpdateVisual();
        }

        private void OnDisable()
        {
            //_buildingPlace.ProgressUpdated -= UpdateVisual;
        }

        private void UpdateVisual()
        {
            //var leftCount = _buildingPlace.LeftCount;
            //leftCount = Mathf.Max(0, leftCount);
            //_progressText.text = leftCount.ToString();
        }
    }
}
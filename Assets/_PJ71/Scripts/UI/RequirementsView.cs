using NavySpade._PJ71.InventorySystem;
using TMPro;
using UnityEngine;

namespace NavySpade._PJ71.UI
{
    public class RequirementsView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _leftResourcesText;
        [SerializeField] private SpriteRenderer _icon;

        private IRequirementsHolder _holder;
        
        public void Init(IRequirementsHolder holder)
        {
            _holder = holder;
            _holder.ProgressUpdated += UpdateProgress;
            _icon.sprite = _holder.Requirement.Preset.Icon;
            UpdateProgress();
        }

        private void OnDestroy()
        {
            if(_holder != null)
                _holder.ProgressUpdated -= UpdateProgress;
        }

        private void UpdateProgress()
        {
            _leftResourcesText.text = _holder.LeftResources.ToString();
        }
    }
}
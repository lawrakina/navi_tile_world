using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using TMPro;
using UnityEngine;

namespace NavySpade.pj77.Tutorial
{
    public class TutorialUIController : MonoSingleton<TutorialUIController>
    {
        [SerializeField] private GameObject _textHolder;
        [SerializeField] private TextMeshProUGUI _textField;

        public void ShowText(string str)
        {
            _textHolder.SetActive(true);
            _textField.text = str;
        }

        public void HideText()
        {
            _textHolder.SetActive(false);
        }
    }
}
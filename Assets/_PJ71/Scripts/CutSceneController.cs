using UnityEngine;


namespace NavySpade._PJ71 {
    public class CutSceneController: MonoBehaviour {
        [SerializeField] private OpeningCutScene _openingCutScene;
        [SerializeField] private bool _showCutSceneAfterStart = false;
        [SerializeField] private KeyCode _keyForEnableCutScene = KeyCode.G;
        private void Awake() {
            _openingCutScene.gameObject.SetActive(_showCutSceneAfterStart);
        }

        private void Update() {
            if (Input.GetKeyDown(_keyForEnableCutScene))
            {
                _openingCutScene.gameObject.SetActive(true);
                _openingCutScene.PlayCutScene();
            }
        }
    }
}
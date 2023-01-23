using UnityEngine;

namespace Core.UI.Popups.Graph
{
    public class UIGraphExecutor : MonoBehaviour
    {
        public PopupsGraph UIGraph;

        private void Awake()
        {
            UIGraph.Initialize();
        }

        private void Start()
        {
            UIGraph.RunFromStart();
        }

        private void OnDestroy()
        {
            UIGraph.Dispose();
        }
    }
}
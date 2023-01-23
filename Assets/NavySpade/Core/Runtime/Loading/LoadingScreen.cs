using System.Collections;
using Core.Loading.Operations;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;

        private OperationExecutor _operationExecutor;
        
        public void Init()
        {
            _operationExecutor = new OperationExecutor();
            
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void Execute(params IAsyncOperation[] asyncOperation)
        {
            StartCoroutine(Executing(asyncOperation));
        }

        private IEnumerator Executing(IAsyncOperation[] asyncOperation)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _image.fillAmount = 0;

            int completedOperations = 0;
            foreach (var operation in asyncOperation)
            {
                yield return _operationExecutor.Execute(operation);
                completedOperations++;
                _image.fillAmount = (float) completedOperations / asyncOperation.Length;
            }
            
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
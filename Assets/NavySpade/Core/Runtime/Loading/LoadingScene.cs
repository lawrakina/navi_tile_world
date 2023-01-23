using System.Collections;
using Core.Extensions;
using Core.Loading.Operations;
using UnityEngine.SceneManagement;

namespace Core.Loading
{
    public class LoadingScene : IAsyncOperation
    {
        private string _sceneName;
        private int _buildIndex = -1;
        
        public Scene LoadedScene { get; private set; }
        
        public LoadingScene(string sceneName)
        {
            _sceneName = sceneName;
        }

        public LoadingScene(int buildIndex)
        {
            _buildIndex = buildIndex;
        }
        
        public IEnumerator Load()
        {
            if (_buildIndex != -1)
            {
                yield return LoadByIndex();
            }
            else
            {
                yield return LoadByName();
            }
            
            SceneManager.SetActiveScene(LoadedScene);
        }

        private IEnumerator LoadByIndex()
        {
            if (SceneExtensions.IsSceneLoaded(_buildIndex) == false)
            {
                var loader = SceneManager.LoadSceneAsync(_buildIndex, LoadSceneMode.Additive);
                while (!loader.isDone)
                {
                    yield return null;
                }
            }
                
            LoadedScene = SceneManager.GetSceneByBuildIndex(_buildIndex);
        }

        private IEnumerator LoadByName()
        {
            if (SceneExtensions.IsSceneLoaded(_sceneName) == false)
            {
                var loader = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
                while (!loader.isDone)
                {
                    yield return null;
                }
            }
                
            LoadedScene = SceneManager.GetSceneByName(_sceneName);
        }
    }
}
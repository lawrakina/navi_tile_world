using Core.Loading.Operations;

namespace Core.Loading.Example
{
    public class LoadingExample
    {
        public void ExecuteExample()
        {
            SplashScreen splashScreen = null;
            
            IAsyncOperation[] _operations = new IAsyncOperation[]
            {
                new LoadingGameScene(),
                new ActionOperation(SomeMethod),
            };

            splashScreen.Execute(_operations);
        }

        private void SomeMethod()
        {
            
        }
    }
}
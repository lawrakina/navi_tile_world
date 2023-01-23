using Core.Input.Commands.Interfaces;
using EventSystem.Runtime.Core.Managers;

namespace Core.Input.Commands
{
    public class PauseResumeCommand : ICommand
    {
        public void Execute()
        {
            EventManager.Invoke(MainEnumEvent.Pause);
        }
    }
}

using Core.Input.Commands.Interfaces;

namespace Core.Input.Readers.Interfaces
{
    public interface IInputReader
    {
        ICommand StartGameCommand { get; set; }
        ICommand PauseResumeCommand { get; set; }
    }
}
using System.Windows.Input;
using IRI.Maptor.Jab.Common.Assets.Commands;

namespace Stateless;

public static class StateMachineExtensions
{
    public static ICommand CreateCommand<TState, TTrigger>(this StateMachine<TState, TTrigger> stateMachine, TTrigger trigger)
    {
        return new RelayCommand
          (
            (param) => stateMachine.Fire(trigger),
            (param) => stateMachine.CanFire(trigger)
          );
    }
}

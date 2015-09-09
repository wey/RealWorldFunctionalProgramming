namespace RealWorldImperativeProgrammingFSharp

open System
open System.Windows.Input


type DelegateCommand (executeAction: Action, canExecute: bool Func) = 

    let canExecuteChanged = new DelegateEvent<EventHandler> ()

    new (executeAction: Action) = DelegateCommand(executeAction, new Func<bool> (fun () -> true))

    interface ICommand with
        [<CLIEvent>]
        member this.CanExecuteChanged = canExecuteChanged.Publish
        member this.Execute _ = executeAction.Invoke ()
        member this.CanExecute _ = canExecute.Invoke ()

namespace RealWorldFunctionalProgrammingFSharp

open FSharp.ViewModule
open FsXaml

open System
open System.ComponentModel
open System.IO
open System.Windows.Forms
open System.Windows.Input


type MainView = XAML<"MainWindow.xaml", true>


module MainViewModelFunctions =

    let loadFiles path =
        if Directory.Exists path 
            then Directory.GetFiles path |> List.ofArray |> List.map FileEntry.create
            else []


    let createCommand (executeAction: unit -> unit) = 
        let canExecuteChanged = new DelegateEvent<EventHandler> ()
        { new ICommand with 
            [<CLIEvent>]
            member this.CanExecuteChanged = canExecuteChanged.Publish
            member this.Execute _ = executeAction ()
            member this.CanExecute _ = true
        }

type MainViewModelState = {
    CurrentPath: string
    Files: FileEntry list
    SelectedFile: FileEntry
    FileFilters: FileFilter list
    SelectedFileFilter: FileFilter
    FilterString: string
    FileActions: FileAction list
    SelectedFileAction: FileAction
    PropertyChangedEvent: Event<PropertyChangedEventHandler, PropertyChangedEventArgs>
}

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MainViewModelState =

    let pickFirstIfAny files = if List.isEmpty files then FileEntry.empty else List.head files

    let create initialPath = 
        let files = MainViewModelFunctions.loadFiles initialPath
        { 
            CurrentPath = initialPath
            Files = files
            SelectedFile = pickFirstIfAny files
            FileFilters = FileFilters.all
            SelectedFileFilter = FileFilters.all |> List.head
            FilterString = String.Empty
            FileActions = FileActions.all
            SelectedFileAction = FileActions.all |> List.head
            PropertyChangedEvent = new Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
        }

    let setCurrentPath path state =
        let files = MainViewModelFunctions.loadFiles path
        { state with CurrentPath = path; Files = files; SelectedFile = pickFirstIfAny files }
    let setFiles files state = { state with Files = files; SelectedFile = pickFirstIfAny files }
    let setSelectedFile file state = { state with SelectedFile = file }
    let setSelectedFileFilter filter state = { state with SelectedFileFilter = filter }
    let setFilterString filterString state = { state with FilterString = filterString }
    let setSelectedFileAction action state = { state with SelectedFileAction = action }

    let onPropertyChanged sender state property = state.PropertyChangedEvent.Trigger (sender, new PropertyChangedEventArgs (property))

    let chooseDirectory state = 
        let dialog = new FolderBrowserDialog();
        if dialog.ShowDialog() = DialogResult.OK 
            then setCurrentPath dialog.SelectedPath state
            else state

    let execute state =
        state.SelectedFileAction.Execute state.SelectedFile.Path
        setFiles (MainViewModelFunctions.loadFiles state.CurrentPath) state

    
type MainViewModel () as this = 
    inherit ViewModelBase ()

    let mutable state = MainViewModelState.create @"C:\"
    
    let changeState action = state <- action state

    let changeStateWithEvents properties action =
        changeState action
        List.iter (MainViewModelState.onPropertyChanged this state) properties

    let changeStateWithEvent property action = changeStateWithEvents [property] action

    let executeCommand = 
        let execute () = changeStateWithEvents ["Files"; "FilteredFiles"; "SelectedFile"] MainViewModelState.execute
        MainViewModelFunctions.createCommand execute
    let chooseDirectoryCommand = 
        let execute () = changeStateWithEvents ["CurrentPath"; "Files"; "FilteredFiles"; "SelectedFile"] MainViewModelState.chooseDirectory
        MainViewModelFunctions.createCommand execute
    
    member this.CurrentPath 
        with get () = state.CurrentPath
        and set value = changeStateWithEvents ["CurrentPath"; "Files"; "FilteredFiles"; "SelectedFile"] (MainViewModelState.setCurrentPath value)
    member this.Files
        with get () = state.Files
        and set value = changeStateWithEvents ["Files"; "FilteredFiles"; "SelectedFile"] (MainViewModelState.setFiles value)
    member this.FilteredFiles
        with get () = state.Files |> List.filter (fun file -> state.SelectedFileFilter.Predicate state.FilterString file.Path)
    member this.SelectedFile
        with get () = state.SelectedFile
        and set value = changeStateWithEvent "SelectedFile" (MainViewModelState.setSelectedFile value)
    member this.FileFilters 
        with get () = state.FileFilters
    member this.SelectedFileFilter 
        with get () = state.SelectedFileFilter
        and set value = changeStateWithEvents ["SelectedFileFilter"; "FilteredFiles"; "SelectedFile"] (MainViewModelState.setSelectedFileFilter value)
    member this.FilterString
        with get () = state.FilterString
        and set value = changeStateWithEvents ["FilterString"; "FilteredFiles"; "SelectedFile"] (MainViewModelState.setFilterString value)
    member this.FileActions 
        with get () = state.FileActions
    member this.SelectedFileAction 
        with get () = state.SelectedFileAction
        and set value = changeStateWithEvent "SelectedFileAction" (MainViewModelState.setSelectedFileAction value)
    member this.ExecuteCommand with get () = executeCommand
    member this.ChooseDirectoryCommand with get () = chooseDirectoryCommand
  
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = state.PropertyChangedEvent.Publish    
    
   
//type MainView () as this =
//    inherit Window ()
//    let (?) (this : Control) (prop : string) : 'T = this.FindName(prop) :?> 'T
//    let uri = new System.Uri("/App;component/MainWindow.xaml", UriKind.Relative)
//    do 
//        Application.LoadComponent(this, uri)


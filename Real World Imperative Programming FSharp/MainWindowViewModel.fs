namespace RealWorldImperativeProgrammingFSharp

open FSharp.ViewModule
open FsXaml

open System
open System.Collections.Generic
open System.ComponentModel
open System.IO
open System.Linq
open System.Windows.Forms


type MainView = XAML<"MainWindow.xaml", true>


type MainViewModel () as this = 
    inherit ViewModelBase ()

    let mutable currentPath = @"C:\"
    let mutable files: FileEntry List = new List<FileEntry> ()
    let mutable selectedFile: FileEntry = null
    let fileFilters = new List<IFileFilter> ([new AcceptAllFilter () :> IFileFilter; new ExtensionFilter () :> IFileFilter; new SubstringFilter () :> IFileFilter])
    let mutable selectedFileFilter = fileFilters.First ()
    let mutable filterString = ""
    let fileActions = new List<IFileAction> ([new OpenFileAction () :> IFileAction; new DeleteFileAction () :> IFileAction]) 
    let mutable selectedFileAction = fileActions.First ()
    let executeCommand = new DelegateCommand (new Action (this.Execute))
    let chooseDirectoryCommand = new DelegateCommand (new Action (this.ChooseDirectory))
    let propertyChangedEvent = new Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    do
        files <- this.LoadFiles currentPath
        selectedFile <- if files.Any () then files.First () else null

    member this.CurrentPath 
        with get () = currentPath
        and set value = 
            currentPath <- value
            this.OnPropertyChanged "CurrentPath"
            this.Files <- this.LoadFiles currentPath
    member this.Files
        with get () = files
        and set value = 
            files <- value
            this.OnPropertyChanged "Files"
            this.OnPropertyChanged "FilteredFiles"
    member this.FilteredFiles
        with get () = files.Where (fun f -> selectedFileFilter.Apply filterString f.Path)
    member this.SelectedFile
        with get () = selectedFile
        and set value =
            selectedFile <- value
            this.OnPropertyChanged "SelectedFile"
    member this.FileFilters 
        with get () = fileFilters
    member this.SelectedFileFilter 
        with get () = selectedFileFilter
        and set value = 
            selectedFileFilter <- value
            this.OnPropertyChanged "SelectedFileFilter"
            this.OnPropertyChanged "FilteredFiles"
    member this.FilterString
        with get () = filterString
        and set value = 
            filterString <- value
            this.OnPropertyChanged "FilterString"
            this.OnPropertyChanged "FilteredFiles"
    member this.FileActions 
        with get () = fileActions
    member this.SelectedFileAction 
        with get () = selectedFileAction
        and set value = 
            selectedFileAction <- value
            this.OnPropertyChanged "SelectedFileAction"
    member this.ExecuteCommand with get () = executeCommand
    member this.ChooseDirectoryCommand with get () = chooseDirectoryCommand
  
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChangedEvent.Publish    
    
    member private this.OnPropertyChanged property = 
        propertyChangedEvent.Trigger (this, new PropertyChangedEventArgs (property))
    member private this.Execute () =
        selectedFileAction.Execute selectedFile.Path
        this.Files <- this.LoadFiles currentPath
    member private this.ChooseDirectory () =
        let dialog = new FolderBrowserDialog();
        if dialog.ShowDialog() = DialogResult.OK then this.CurrentPath <- dialog.SelectedPath
    member private this.LoadFiles path =
        if Directory.Exists path 
            then ((Directory.GetFiles path).Select (fun p -> new FileEntry (p))).ToList ()
            else new List<FileEntry> ()


//type MainView () as this =
//    inherit Window ()
//    let (?) (this : Control) (prop : string) : 'T = this.FindName(prop) :?> 'T
//    let uri = new System.Uri("/App;component/MainWindow.xaml", UriKind.Relative)
//    do 
//        Application.LoadComponent(this, uri)


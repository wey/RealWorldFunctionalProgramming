namespace RealWorldImperativeProgrammingFSharp

open System.IO
open System.Diagnostics


type IFileAction =
   abstract member Execute: string -> unit


type OpenFileAction () =

    member this.Name = "Datei öffnen"

    interface IFileAction with
        member this.Execute path =
            if File.Exists path then
                try Process.Start path |> ignore
                with _ -> ()


type DeleteFileAction () =

    member this.Name = "Datei löschen"

    interface IFileAction with
        member this.Execute path = 
            if File.Exists path then
                try File.Delete path
                with _ -> ()

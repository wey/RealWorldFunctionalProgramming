namespace RealWorldFunctionalProgrammingFSharp

open System.IO
open System.Diagnostics


type FileAction = { Name: string; Execute: string -> unit }

module FileActions =
    
    let openFile path = 
        if File.Exists path then
            try Process.Start path |> ignore
            with _ -> ()

    let deleteFile path = 
        if File.Exists path then
            try File.Delete path
            with _ -> ()

    let create name action = { Name = name; Execute = action }

    let openFileAction = create "Datei öffnen" openFile

    let deleteFileAction = create "Datei löschen" deleteFile

    let all = [openFileAction; deleteFileAction]

namespace RealWorldFunctionalProgrammingFSharp

open System
open System.IO


type FileFilter = { Name: string; Predicate: string -> string -> bool }

module FileFilters =

    let extensionPredicate filterText file = 
        let extension = Path.GetExtension file
        filterText = if String.IsNullOrEmpty extension then String.Empty else extension.Substring 1

    let substringPredicate filterText file = (Path.GetFileName file).Contains filterText

    let create name predicate = { Name = name; Predicate = predicate }

    let acceptAllFilter = create "Kein Filter" (fun _ _ -> true)
    
    let extensionFilter = create "Dateierweiterungsfilter" extensionPredicate
    
    let substringFilter = create "Teilstring-Filter" substringPredicate

    let all = [acceptAllFilter; extensionFilter; substringFilter]

namespace RealWorldImperativeProgrammingFSharp

open System.IO


type IFileFilter =
    abstract member Apply: string -> string -> bool


type AcceptAllFilter () =
    
    member this.Name with get () = "Kein Filter"
    
    interface IFileFilter with
        member this.Apply _ _ = true

    
type ExtensionFilter () =
    
    member this.Name with get () = "Dateierweiterungsfilter"

    interface IFileFilter with
        member this.Apply filterText file = 
            let extension = (Path.GetExtension file).Substring 1
            extension = filterText
    

type SubstringFilter () =
    
    member this.Name with get () = "Teilstring-Filter"
    
    interface IFileFilter with
        member this.Apply filterText file = (Path.GetFileName file).Contains filterText  

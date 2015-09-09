namespace RealWorldFunctionalProgrammingFSharp

open System
open System.IO


type FileEntry = { Path: string; Name: string }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FileEntry =
   
    let create path = { Path = path; Name = Path.GetFileName path } 
   
    let empty = create String.Empty       

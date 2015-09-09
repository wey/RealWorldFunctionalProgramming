namespace RealWorldImperativeProgrammingFSharp

open System.IO


[<AllowNullLiteral>]
type FileEntry (path) =
    member this.Path with get () = path
    member this.Name with get () = Path.GetFileName path
        
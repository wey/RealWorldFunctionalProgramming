namespace UnitTestProject1

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open RealWorldFunctionalProgrammingFSharp

[<TestClass>]
type UnitTest () = 

    [<TestMethod>]
    member this.``When a FileEntry is created, the Name property should correspond to the file name without path`` () =
        let fileEntry = FileEntry.create @"C:\Temp\test.txt"
        let expectedFileName = "test.txt"
        Assert.AreEqual (expectedFileName, fileEntry.Name)

    [<TestMethod>]
    member this.``When the files of the view model are set, the SelectedFile property points to the first of them`` () =
        let viewModel = new MainViewModel ()
        let paths = [@"c:\Temp\test1.txt"; @"c:\Temp\test2.txt"; @"c:\Temp\test3.txt"; @"c:\Temp\test4.txt"; @"c:\Temp\test5.txt"]
        let fileEntries = List.map FileEntry.create paths
        viewModel.Files <- fileEntries
        Assert.AreEqual (List.head fileEntries, viewModel.SelectedFile)

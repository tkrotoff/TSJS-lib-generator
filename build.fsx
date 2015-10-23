#r "packages/FAKE/tools/FakeLib.dll"
#load "Shared.fsx"
#load "TS.fsx"

open Fake
open TS
open System
open System.IO

Target "Run" (fun _ ->
    // For typescript only generate for Dom
    TS.EmitDomWeb()
    TS.EmitDomWorker()
)

let testFile file =
    let baseline = "./baselines/" + file
    let newFile = "./generated/" + file
    if FilesAreEqual (FileInfo baseline) (FileInfo newFile) then
        String.Empty
    else
        sprintf "Test failed: %s is different from baseline file.\n" newFile

Target "Test" (fun _ ->
    Directory.GetFiles("./baselines")
    |> Array.map (Path.GetFileName >> testFile)
    |> String.concat ""
    |> (fun msg -> if String.IsNullOrEmpty(msg) then tracefn "All tests passed." else failwith msg)
)

"Run" ==> "Test"

RunTargetOrDefault "Test"

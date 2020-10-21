module StudyFSharp.Query

open System
open System.Diagnostics

type Customer =
    { State : string; ZipCode : int }

let allCustomers = [ {State = "ZH"; ZipCode = 000}; {State = "JP"; ZipCode = 001} ]
    
let customZipCodesByState stateName =
    allCustomers
    |> Seq.filter (fun customer -> customer.State = stateName)
    |> Seq.map (fun customer -> customer.ZipCode)
    |> Seq.distinct

let customZipCodesByState2 stateName =
    query {
        for customer in allCustomers do
        where (customer.State = stateName)
        select customer.ZipCode
        distinct
    }

printfn "result: %A" <| customZipCodesByState2 "JP"

let activeProcCount =
    query {
        for activeProc in Process.GetProcesses() do
        count
    }
    
let memoryHog =
    query {
        for activeProcess in Process.GetProcesses() do
        sortByDescending activeProcess.WorkingSet64
        head
    }

printfn "'%s' has a working set of:\n%d bytes" memoryHog.MainWindowTitle memoryHog.WorkingSet64

let windowedProcesses =
    query {
        for activeProcess in Process.GetProcesses() do
        where (activeProcess.MainWindowHandle <> nativeint 0)
        select activeProcess
    }

let printProcessList procSeq =
    Seq.iter (printfn "%A") procSeq

printProcessList windowedProcesses

let isChromeRunning =
    query {
        for windowedProc in windowedProcesses do
        select windowedProc.ProcessName
        contains "chrome"
    }

let numOfServiceProcesses =
    query {
        for activeProcess in Process.GetProcesses() do
        where (activeProcess.MainWindowHandle = nativeint 0)
        select activeProcess
        count
    }

printfn "isChromeRunning: %A" isChromeRunning
printfn "numOfServiceProcesses: %A" numOfServiceProcesses

let oneHundredNumbersUnderFifty =
    let rng = Random()
    seq {
        for i = 1 to 100 do
            yield rng.Next() % 50
    }
    
let distinctNumbers =
    query {
        for randomNumber in oneHundredNumbersUnderFifty do
        select randomNumber
        distinct
    }

printfn "result: %A" oneHundredNumbersUnderFifty
printfn "%d distinct numbers found." <| Seq.length distinctNumbers

// maxBy
let highestThreadCount =
    query {
        for proc in Process.GetProcesses() do
        maxBy proc.Threads.Count
    }

// sortBy, thenBy
let sortedProcs =
    query {
        for proc in Process.GetProcesses() do
        let isWindowed = proc.MainWindowHandle <> nativeint 0
        sortBy isWindowed
        thenBy proc.ProcessName
        select proc
    }

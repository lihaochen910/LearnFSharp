module StudyFSharp.Exceptions

open System
open System.Collections.Generic
open System.IO

// 自定义简单异常
exception NoMagicWand
exception NoFullMoon of int * int
exception BadMojo of string

// 使用failwithf
let divide x y =
    if y = 0 then failwithf "Cannot divide %d by zero!" x
    x / y

//    divide 10 0

let divide2 x y =
    if y = 0 then raise <| System.DivideByZeroException()
    x / y

// 异常处理
let exitCode =
    let args = [| "." |]
    try
        let filePath = args.[0]
        
        printfn "Trying to gather information about file:"
        printfn "%s" filePath
        
        // Does the drive exist?
        let matchingDrive =
            Directory.GetLogicalDrives()
            |> Array.tryFind (fun drivePath -> drivePath.[0] = filePath.[0])
        
        if matchingDrive = None then
            raise <| DriveNotFoundException(filePath)

        // Does the folder exist?
        let directory = Path.GetPathRoot(filePath)
        if not <| Directory.Exists(directory) then
            raise <| DirectoryNotFoundException(filePath)

        // Does the file exist?
        if not <| File.Exists(filePath) then
            raise <| FileNotFoundException(filePath)

        let fileInfo = FileInfo(filePath)
        printfn "Created = %s" <| fileInfo.CreationTime.ToString()
        printfn "Access = %s" <| fileInfo.LastAccessTime.ToString()
        printfn "Size = %d" fileInfo.Length
        
        0
        
    with
    // Combine patterns using Or
    | :? DriveNotFoundException
    | :? DirectoryNotFoundException
        -> printfn "Unhandled Drive or Directory not found exception"
           1
    // Bind the exception value to value ex
    | :? FileNotFoundException as ex
        -> printfn "Unhandled FileNotFoundException: %s" ex.Message
           3
    | :? IOException as ex
        -> printfn "Unhandled IOException: %s" ex.Message
           4
    // Use a wild card match (ex will be of type System.Exception)
    | _ as ex
        -> printfn "Unhandled Exception: %s" ex.Message
           5
           
// Return the exit code
printfn "Exiting with code %d" exitCode
//    exitCode

// Try–finally 表达式
let tryFinallyTest() =
    try
        printfn "Before exception..."
        failwith "ERROR!"
        printfn "After exception raised..."
    finally
        printfn "Finally block executing..."

let test() =
    try
        tryFinallyTest()
    with
    | ex -> printfn "Exception caught with message: %s" ex.Message

test()

// Reraising Exceptions
let tryWithBackoff f times =
    let mutable attempt = 1
    let mutable success = false
    
    while not success do
        try
            f()
            success <- true
        with ex ->
            attempt <- attempt + 1
            if attempt >= times then
                reraise()

// 自定义简单异常
let castHex (ingredients : HashSet<string>) =
    let isFullMoon day = false
    try
        let currentWand = Environment.OSVersion
        if currentWand = null then
            raise NoMagicWand
        
        if not <| ingredients.Contains("Toad Wart") then
            raise <| BadMojo("Need Toad Wart to cast the hex!")
            
        if not <| isFullMoon(DateTime.Today) then
            raise <| NoFullMoon(DateTime.Today.Month, DateTime.Today.Day)

        // Begin the incantation...
        let mana =
            ingredients
            |> Seq.map (fun i -> i.GetHashCode())
            |> Seq.fold (+) 0

        sprintf "%x" mana

    with
    | NoMagicWand
        -> "Error: A magic wand is required to hex!"
    | NoFullMoon(month, day)
        -> "Error: Hexes can only be cast during a full moon."
    | BadMojo(msg)
        -> sprintf "Error: Hex failed due to bad mojo [%s]" msg

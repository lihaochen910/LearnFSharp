// Learn more about F# at http://fsharp.org
open System
open System.Collections.Generic
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns

[<EntryPoint>]
let main (args : string[]) =
    
    printfn "%A" <@ 1 + 1 @>
    printfn "%A" <@@ fun x -> "Hello, " + x @@>
    
    let rec describeCode (expr : Expr) =
        match expr with
        | Int32(i) -> printfn "Integer with value %d" i
        | Double(f) -> printfn "Floating-point with value %f" f
        | String(s) -> printfn "String with value %s" s
        | Call(calledOnObject, methInfo, args)
            -> let calledOn = match calledOnObject with
                                | Some(x) -> sprintf "%A"x
                                | None -> "Called a static method"

               printfn "Calling method '%s': \n On value: %s \n With args: %A" methInfo.Name calledOn args
        | Lambda(var, lambdaBody) ->
            printfn
                "Lambda Expression - Introduced value %s with type %s"
                var.Name var.Type.Name
            printfn "Processing body of Lambda Expression..."
            describeCode lambdaBody
        | _ -> printfn "Unknown expression form:\n%A" expr
        
        
    describeCode <@ "a string".ToUpper @>
    
    0 // return an integer exit code

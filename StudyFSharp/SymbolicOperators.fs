module StudyFSharp.SymbolicOperators

open System

// 符号
let rec (!) x =
    if x <= 1 then 1
    else x * !(x - 1)
    
printfn "result %A" (!3)

// 计算列表中所有数字的总和
printfn "result %A" (List.fold (+) 0 [1 .. 100])

// 计算列表中所有数字的乘积
printfn "result %A" (List.fold (*) 1 [1 .. 100])

let minus = (-)
List.fold minus 10 [3; 3; 3]
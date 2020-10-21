module StudyFSharp.RecursiveFunctions

open System

// 定义一个递归方法
let rec factorial x =
    if x <= 1 then
        1
    else
        x * factorial (x - 1)

printfn "result %A" (factorial 5)

// 互相递归
let rec isOdd x =
    if x = 0 then false
    elif x = 1 then true
    else isEven(x - 1)
and isEven x =
    if x = 0 then true
    elif x = 1 then false
    else isOdd(x - 1)
        
printfn "result %A" (isOdd 50)
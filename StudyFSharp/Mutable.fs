module StudyFSharp.Mutable

open System

type Thing = Plant | Animal | Mineral

type MutableCar = { Make : string; Model : string; mutable Miles : int }

let isNull = function null -> true | _ -> false
    
printfn "result: %A" <| isNull "a string"
printfn "result: %A" <| isNull (null : string)

let testThing thing =
    match thing with
    | Plant -> "Plant"
    | Animal -> "Animal"
    | Mineral -> "Mineral"

let x = [| 0 |]
let y = x

x.[0] <- 3
printfn "x: %A" x
printfn "y: %A" y

// 使用mutable关键字创建一个可变的值
let mutable message = "World"
printfn "Hello, %s" message

message <- "Universe"
printfn "Hello, %s" message

// F# 3.0书中的编译器版本不支持在闭包中修改可变值
// * 可变值不能从函数返回
// * 不能在内部函数(闭包)中获取可变值
// 测试时的Mono编译器支持在闭包中修改上层的可变值
let invalidUseOfMutable() =
    let mutable x = 0
    
    let incrementX() = x <- x + 1
    incrementX()
    
    x
    
printfn "result: %A" <| invalidUseOfMutable()

let planets =
    ref [
        "Mercury"; "Venus"; "Earth";
        "Mars"; "Jupiter"; "Saturn";
        "Uranus"; "Neptune"; "Pluto"
    ]

// 使用!符号获取目标的引用
// 赋予新值使用符号:=
planets := !planets |> List.filter (fun p -> p <> "Pluto")
printfn "planets: %A" planets

// 在F#中注意符号!不是布尔运算, 而是取引用
let b = ref true
printfn "b: %A" !b

// F#库中使用incr自增int ref的值, 使用decr自减int ref的值
let n = ref 0
incr n
printfn "n: %A" !n
decr n
printfn "n: %A" !n

// 修改记录中的可变字段
let driveForASeason car =
    let rng = Random()
    car.Miles <- car.Miles + rng.Next() % 10000

let kitt = { Make = "Pontiac"; Model = "Trans Am"; Miles = 0 }

driveForASeason kitt
driveForASeason kitt

printfn "result: %A" kitt
module StudyFSharp.FunctionValues

open System

// 高阶函数示例
let negate x = -x
List.map negate [1 .. 10]

// 匿名函数
List.map (fun i -> -i) [1 .. 10]

// 函数返回函数
// 生成函数的函数
let generatePowerOfFunc baseValue =
    (fun exponent -> baseValue ** exponent)

let powerOfTwo = generatePowerOfFunc 2.0
let powerOfThree = generatePowerOfFunc 3.0
let powerOfThreeOne = generatePowerOfFunc 31.0
printfn "result %A" (powerOfThreeOne 2.0)
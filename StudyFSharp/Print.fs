module StudyFSharp.Print

open System

// printf and printfn
printf "Hello, "
printfn "World"

// output: Hello, World

let mountain = "K2"
let height = 8611
let units = 'm'

printfn "%s is %d%c high" mountain height units

// output: K2 is 28251m high

// sprintf返回类型为string
let location = "World"
let str = sprintf "Hello, %s" location

// Printf format specifiers
// %d, %i 整数
printf "%d" 5 // 5

// %x, %o 十六进制、八进制整数
printfn "%x" 255 // ff

// %s 字符串
printf "%s" "ABC" // ABC

// %f 浮点数
printf "%f" 1.1M // 1.100000

// %c 字符
printf "%c" '\097' // a

// %b 布尔
printf "%b" false // false

// %O 任意Object 将会调用Object.ToString虚方法
printfn "%O" (1,2) // (1, 2)

// %A Anything
printf "%A" (1, []) // (1, [])
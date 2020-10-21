module StudyFSharp.PatternMatching

open System

// 使用match匹配
let isOdd x = (x % 2 = 1)

let describeNumber x =
    match isOdd x with
    | true  -> printfn "%A is odd" x
    | false -> printfn "%A is even" x
    
describeNumber 4

// Truth table for AND via pattern matching
let testAnd x y =
    match x, y with
    | true, true -> true
    | true, false -> false
    | false, true -> false
    | false, false -> false
    
printfn "result %A" <| testAnd true true

// 下划线可以匹配任意
let testAndSimplified x y =
    match x, y with
    | true, true -> true
    | _, _ -> printfn "Any"; false
    
printfn "result %A" <| testAndSimplified true false

// 不完整的匹配, 编译器将会发出警告
let testAndIncompleted x y =
    match x, y with
    | true, true -> true
    | true, false -> false
//        | false, true -> false
    | false, false -> false

testAndIncompleted true true

// 命名匹配
let greet name =
    match name with
    | "Robert" -> printfn "Hello, Bob"
    | "William" -> printfn "Hello, Bill"
    | x -> printfn "Hello, %s" x
//        | _ -> printfn "Hello Any, %s" name
    
greet "Earl"

// 使用Literal标签消除歧义
//    [<Literal>]
let Bill = "Bill Gates"

let greet2 name =
    match name with
    | Bill -> "Hello Bill!"
    | x -> sprintf "Hello, %s" x

// 使用when匹配的例子: 猜数字游戏
let highLowGame () =
    
    let rng = Random()
    let secretNumber = rng.Next() % 100
    
    let rec highLowGameStep () =
        
        printfn "Guess the secret number:"
        let guessStr = Console.ReadLine()
        let guess = Int32.Parse guessStr
        
        match guess with
        | _ when guess > secretNumber
            -> printfn "The secret number is lower."
               highLowGameStep()
               
        | _ when guess = secretNumber
            -> printfn "You've guessed correctly!"
               ()
        
        | _ when guess < secretNumber
            -> printfn "The secret number is higher."
               highLowGameStep()

    highLowGameStep()
    
//    highLowGame()

let vowelTest c =
    match c with
    | 'a' | 'e' | 'i' | 'o' | 'u'
        -> true
    | _ -> false
    
let describeNumbers x y =
    match x, y with
    | 1, _
    | _, 1
        -> printfn "One of the numbers is one."
    | (2, _) & (_, 2)
        -> printfn "Both of the numbers are two."
    | _ -> printfn "Other."
    
describeNumbers 2 2

let testXor x y =
    match x, y with
    | tuple when fst tuple <> snd tuple
        -> true
    | true, true -> false
    | false, false -> false

let rec listLength l =
    match l with
    | [] -> 0
    | [_] -> 1
    | [_; _] -> 2
    | [_; _; _] -> 3
    | hd :: tail -> 1 + listLength tail

let rec funListLength =
    function
    | [] -> 0
    | [_] -> 1
    | [_; _] -> 2
    | [_; _; _] -> 3
    | hd :: tail -> 1 + listLength tail

let describeOption o =
    match o with
    | Some(42) -> printfn "The answer was 42, but what was the question?"
    | Some(x) -> printfn "The answer was %d" x
    | None -> printfn "No answer found."

describeOption

// 通配符
List.iter (fun _ -> printfn "Step...") [1 .. 3]

let _, second, _ = (1, 2, 3)
module StudyFSharp.Arrays

open System

let perfectSquares = [| for i in 1 .. 7 -> i * i |]
let perfectSquares2 = [| 1; 4; 9; 16; 25; 36; 49 |]

printfn
    "The first three perfect squares are %d, %d, and %d"
    perfectSquares.[0]
    perfectSquares.[1]
    perfectSquares.[2]

let rot13Encrypt (letter : char) =
    
    if Char.IsLetter(letter) then
        let newLetter =
            (int letter)
            |> (fun letterIdx -> letterIdx - (int 'A'))
            |> (fun letterIdx -> (letterIdx + 13) % 26)
            |> (fun letterIdx -> letterIdx + (int 'A'))
            |> char
        newLetter
    else
        letter

let encryptText (text : char[]) =
    
    for idx = 0 to text.Length - 1 do
        let letter = text.[idx]
        text.[idx] <- rot13Encrypt letter

let text =
    Array.ofSeq "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG"

printfn "Origional = %s" <| String(text)
encryptText text
printfn "Encrypted = %s" <| String(text)

encryptText text
printfn "Decrypted = %s" <| String(text)

// Array slices
let daysOfWeek = Enum.GetNames( typeof<DayOfWeek> )

printfn "2..4: %A" daysOfWeek.[2..4]
printfn "4..: %A" daysOfWeek.[4..]
printfn "..2: %A" daysOfWeek.[..2]
printfn "*: %A" daysOfWeek.[*]

// 数组初始化
let divisions = 4.0
let twoPi = 2.0 * Math.PI

Array.init (int divisions) (fun i -> float i * twoPi / divisions)

let emptyIntArray : int[] = Array.zeroCreate 3
let emptyStringArray : string[] = Array.zeroCreate 3

// Array Pattern Matching
let describeArray arr =
    match arr with
    | null -> "The array is null"
    | [| |] -> "The array is empty"
    | [| x |] -> sprintf "The array has one element, %A" x
    | [| x; y |] -> sprintf "The array has two element, %A and %A" x y
    | a -> sprintf "The array had %d elements, %A" a.Length a

// Array Equality
printfn "%A" <| describeArray [| 1 .. 2 |]
printfn "%A" <| describeArray [| ("tuple", 1, 2, 3) |]

printfn "%A" <| ([| 1 .. 5 |] = [| 1; 2; 3; 4; 5 |])
printfn "%A" <| ([| 1 .. 3 |] = [| |])

// Array.partition
let isGreaterThanTen x = (x > 10)

printfn "result: %A" <|
    ([| 5; 5; 6; 20; 1; 3; 7; 11 |]
    |> Array.partition isGreaterThanTen)

// Array.tryFind
let rec isPowerOfTwo x =
    if x = 2 then
        true
    elif x % 2 = 1 then
        false
    else isPowerOfTwo (x / 2)

printfn "result: %A" <|
    ([| 1; 7; 13; 64; 32 |]
     |> Array.tryFind isPowerOfTwo)

printfn "result index: %A" <|
    ([| 1; 7; 13; 64; 32 |]
     |> Array.tryFindIndex isPowerOfTwo)

// Array.iteri
let vowels = [| 'a'; 'e'; 'i'; 'o'; 'u' |]

Array.iteri (fun idx chr -> printfn "vowel.[%d] = %c" idx chr) vowels

// 多维数组
let identityMatrix : float[,] = Array2D.zeroCreate 3 3
identityMatrix.[0,0] <- 1.0
identityMatrix.[1,1] <- 1.0
identityMatrix.[2,2] <- 1.0

printfn "result: %A" identityMatrix.[*, 1..2]

// 交错数组
let jaggedArray : int[][] = Array.zeroCreate 3
jaggedArray.[0] <- Array.init 1 (fun x -> x)
jaggedArray.[1] <- Array.init 2 (fun x -> x)
jaggedArray.[2] <- Array.init 3 (fun x -> x)

printfn "jaggedArray: %A" jaggedArray

// FSharpList
// Con ::
let x = [2; 3; 4]

// 将元素添加到列表的头部, 并返回一个新的list
let newX = 1 :: x

// Append @
let y = [5; 6; 7]

// 将两个列表拼接起来, 并返回一个新的列表
let newY = x @ y

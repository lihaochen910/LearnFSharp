module StudyFSharp.LazyAndSeq

open System
open System.IO

// 定义两个惰性值
let x = Lazy<int>.Create(fun () -> printfn "Evaluating x..."; 10)
let y = lazy (printfn "Evaluating y..."; x.Value + x.Value)

// 首次访问y值时将会强制计算值
printfn "y: %A" y.Value

// 再次访问y值时将会使用缓存值
printfn "again y: %A" y.Value

let seqOfNumbers = seq { 1 .. 5 }
seqOfNumbers |> Seq.iter (printfn "%d")

// Sequence与List的区别
// Sequence在必要时分配内存
let allPositiveIntsSeq =
    seq { for i in 1 .. Int32.MaxValue do
              yield i }
    
printfn "allPositiveIntsSeq: %A" allPositiveIntsSeq

// List这样将会立即分配内存
//    let allPositiveIntsList = [ for i in 1 .. Int32.MaxValue -> i ]

let alphabet = seq { for c in 'A' .. 'Z' -> c }

printfn "result: %A" <| Seq.take 4 alphabet

// Sequence的副作用
let noisyAlphabet =
    seq {
        for c in 'A' .. 'Z' do
            printfn "Yielding %c..." c
            yield c
    }
    
let fifthLetter = Seq.item 4 noisyAlphabet
printfn "result: %A" fifthLetter

let rec allFilesUnder basePath =
    seq {
        yield! Directory.GetFiles(basePath)
        
        for subdir in Directory.GetDirectories(basePath) do
            yield! allFilesUnder subdir
    }

printfn "result: %A" <| Seq.take 2 (allFilesUnder Environment.CurrentDirectory)

// Seq.take
let randomSequence =
    seq {
        let rng = Random()
        while true do
            yield rng.Next()
    }

printfn "result: %A" <| (Seq.take 10 randomSequence |> Seq.toList)

// Seq.unfold
let nextFibUnder100 (a, b) =
    if a + b > 100 then
        None
    else
        let nextValue = a + b
        Some(nextValue, (nextValue, a))

let fibsUnder100 = Seq.unfold nextFibUnder100 (0, 1)

printfn "result: %A" <| Seq.toList fibsUnder100

// Seq.iter
let oddsUnderN n = seq { for i in 1 .. 2 .. n -> i }
Seq.iter (printfn "%d") (oddsUnderN 10)

// Seq.map
let words = "The quick brown fox jumped over the lazy dog".Split([| ' ' |])

printfn "result: %A" (words |> Seq.map (fun word -> word, word.Length))

// Seq.fold
printfn "result: %A" (Seq.fold (+) 0 <| seq { 1 .. 100 })

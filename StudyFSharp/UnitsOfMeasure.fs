module StudyFSharp.UnitsOfMeasure

open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<Measure>]
type fahrenheit

[<Measure>]
type celsius

[<Measure>]
type m

// Define seconds and hertz
[<Measure>]
type s

[<Measure>]
type Hz = s ^ -1

// 计量单位添加方法
[<Measure>]
type far =
    static member ConvertToCel(x : float<far>) =
        (5.0<cel> / 9.0<far>) * (x - 32.0<far>)
and [<Measure>] cel =
    static member ConvertToFar(x : float<cel>) =
        (9.0<far> / 5.0<cel> * x) + 32.0<far>

// 泛型单位
type Point< [<Measure>] 'u >(x : float<'u>, y : float<'u>) =
    
    member this.X = x
    member this.Y = y
    
    member this.UnitlessX = float x
    member this.UnitlessY = float y

    member this.Length =
        let sqr x = x * x
        sqrt <| sqr this.X + sqr this.Y
        
    override this.ToString() =
        sprintf
            "{%f, %f}"
            this.UnitlessX
            this.UnitlessY

// 计量单位转换
[<Measure>]
type rads

let printTemperature (temp : float<fahrenheit>) =
    if temp < 32.0<_> then
        printfn "Below Freezing!"
    elif temp < 65.0<_> then
        printfn "Cold"
    elif temp < 75.0<_> then
        printfn "Just right!"
    elif temp < 100.0<_> then
        printfn "Hot!"
    else
        printfn "Scorching!"

let seattle = 59.0<fahrenheit>

printTemperature seattle

let cambridge = 18.0<celsius>

//    printTemperature cambridge

1.0<m> * 1.0<m> |> ignore
1.0<m> / 1.0<m> |> ignore
1.0<m> / 1.0<m> / 1.0<m> |> ignore

3.0<s ^ -1> = 3.0<Hz> |> ignore

printfn "result: %A" <| far.ConvertToCel(100.0<far>)

let flaslightIntensity = 80.0<cd>
let world'sLargestGoldNugget = 280.0<kilogram>

// 单位转换
let halfPI = System.Math.PI * 0.5<rads>
sin (float halfPI) |> ignore

// 通用单位
let squareMeter (x : float<m>) = x * x
let genericSquare (x : float<_>) = x * x

printfn "result: %A" <| genericSquare 1.0<m/s>
printfn "result: %A" <| genericSquare 9.0

let p = Point<m>(10.0<m>, 10.0<m>)
printfn "result: %A" p.Length

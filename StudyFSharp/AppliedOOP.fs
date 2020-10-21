module StudyFSharp.AppliedOOP

open System
open System.Collections.Generic
open System.IO

// 运算符重载
(*
    F#可以为类定义任意运算符重载, 比如( +-+, -+- )
    但是与C#, VB兼容的符号如下:
        Binary operators:
           +, -, *, /, %, &, |, <<, >>
           +=, -=, *=, /=, %=
        Unary operators:
           ~+, ~-, ~!, ~++, ~--
*)
[<Measure>]
type ml

type Bottle(capacity : float<ml>) =
    
    new() = new Bottle(0.0<ml>)
    member this.Volume = capacity
    
    static member (+) ((lhs : Bottle), rhs) =
        new Bottle(lhs.Volume + rhs)
        
    static member (-) ((lhs : Bottle), rhs) =
        new Bottle(lhs.Volume - rhs)
        
    static member (~-) (rhs : Bottle) =
        new Bottle(rhs.Volume * -1.0<1>)
        
    override this.ToString() =
        sprintf "Bottle(%.1fml)" (float capacity)

// 索引器, 通过定义名为Item的成员来实现.[]功能
type Year(year : int) =
    member this.Item (idx : int) =
        if idx < 1 || idx > 365 then failwith "Invalid day range"
        let dateStr = sprintf "1-1-%d" year
        DateTime.Parse(dateStr).AddDays(float (idx - 1))

// 索引器读写
type WordBuilder(startingLetters : string) =

    let m_letters = new List<char>(startingLetters)
    
    member this.Item
        with get idx = m_letters.[idx]
        and set idx c = m_letters.[idx] <- c
    
    member this.Word = new string (m_letters.ToArray())

// 切片 ..
type TextBlock(text : string) =
    let words = text.Split([| ' ' |])
    member this.AverageWordLength =
        words |> Array.map float |> Array.average
    
    member this.GetSlice(lowerBound : int option, upperBound : int option) =
        let words =
            match lowerBound, upperBound with
            // Specify both upper and lower bounds
            | Some(lb), Some(ub) -> words.[lb..ub]
            // Just one bound specified
            | Some(lb), None -> words.[lb..]
            | None, Some(ub) -> words.[..ub]
            // No lower or upper bounds
            | None, None -> words.[*]
        
        words

// 定义二维切片
type DataPoints(points : seq<float * float>) =
    member this.GetSlice(xlb, xub, ylb, yub) =
        let getValue optType defaultValue =
            match optType with
            | Some(x) -> x
            | None -> defaultValue

        let minX = getValue xlb Double.MinValue
        let maxX = getValue xub Double.MaxValue
        let minY = getValue ylb Double.MinValue
        let maxY = getValue yub Double.MaxValue

        // Return if a given tuple representing a point is within range
        let inRange (x, y) =
            ( minX < x && x < maxX &&
                minY < y && y < maxY )

        Seq.filter inRange points

// 定义1000个随机点，值在0.0到1.0之间
let points =
    seq {
        let rng = new Random()
        for i = 0 to 1000 do
            let x = rng.NextDouble()
            let y = rng.NextDouble()
            yield (x, y)
    }

let d = new DataPoints(points)

// 获取x和y值大于0.5的所有值
printfn "result: %A" <| d.[0.5.., 0.5..]

printfn "result: %A" <| d.[0.90..0.99, *]

// 定义委托
let functionValue x y =
    printfn "x = %d, y = %d" x y
    x + y

type DelegateType = delegate of int * int -> int

let delegateValue1 =
    new DelegateType(
        fun x y ->
            printfn "x = %d, y = %d" x y
            x + y
    )

let functionResult = functionValue 1 2
let delegateResult = delegateValue1.Invoke(1, 2)

type IntDelegate = delegate of int -> unit

type ListHelper =
    /// Invokes a delegate for every element of a list
    static member ApplyDelegate (l : int list, d : IntDelegate) =
        l |> List.iter (fun x -> d.Invoke(x))

// Explicitly constructing the delegate
ListHelper.ApplyDelegate([1 .. 10], new IntDelegate(fun x -> printfn "%d" x))

// Implicitly constructing the delegate
ListHelper.ApplyDelegate([1 .. 10], (fun x -> printfn "%d" x))

// 合并委托
type LogMessage = delegate of string -> unit
let printToConsole =
    LogMessage(fun msg -> printfn "Logging to console: %s..." msg)
let appendToLogFile =
    LogMessage(fun msg ->
                    printfn "Logging to file: %s..." msg
                    use file = new StreamWriter("Log.txt", true)
                    file.WriteLine(msg))

let doBoth = LogMessage.Combine(printToConsole, appendToLogFile)
let typedDoBoth = doBoth :?> LogMessage

typedDoBoth.Invoke("[some important message]")

// Events
type SetAction = Added | Removed

type SetOperationEventArgs<'a>(value : 'a, action : SetAction) =
    inherit System.EventArgs()
    member this.Action = action
    member this.Value = value

type SetOperationDelegate<'a> = delegate of obj * SetOperationEventArgs<'a> -> unit

// items are added.
type NoisySet<'a when 'a : comparison>() =
    let mutable m_set = Set.empty : Set<'a>

    let m_itemAdded =
        new Event<SetOperationDelegate<'a>, SetOperationEventArgs<'a>>()

    let m_itemRemoved =
        new Event<SetOperationDelegate<'a>, SetOperationEventArgs<'a>>()

    member this.Add(x) =
        m_set <- m_set.Add(x)
        // Fire the 'Add' event
        m_itemAdded.Trigger(this, new SetOperationEventArgs<_>(x, Added))

    member this.Remove(x) =
        m_set <- m_set.Remove(x)
        // Fire the 'Remove' event
        m_itemRemoved.Trigger(this, new SetOperationEventArgs<_>(x, Removed))

    // Publish the events so others can subscribe to them
    member this.ItemAddedEvent = m_itemAdded.Publish
    member this.ItemRemovedEvent = m_itemRemoved.Publish

let s = new NoisySet<int>()

let setOperationHandler =
    new SetOperationDelegate<int>(
        fun sender args ->
            printfn "%d was %A" args.Value args.Action
    )

s.ItemAddedEvent.AddHandler(setOperationHandler)
s.ItemRemovedEvent.AddHandler(setOperationHandler)

s.Add(9)
s.Remove(9)

// DelegateEvent
type ClockUpdateDelegate = delegate of int * int * int -> unit

type Clock() =
    let m_event = new DelegateEvent<ClockUpdateDelegate>()

    member this.Start() =
        printfn "Started..."
        while true do
            // Sleep one second...
            Threading.Thread.Sleep(1000)
            
            let hour = DateTime.Now.Hour
            let minute = DateTime.Now.Minute
            let second = DateTime.Now.Second
            
            m_event.Trigger( [| box hour; box minute; box second |] )

    member this.ClockUpdate = m_event.Publish
    
let c = new Clock()

c.ClockUpdate.AddHandler(
    new ClockUpdateDelegate(
        fun h m s -> printfn "[%d:%d:%d]" h m s
    ) )
c.Start()

// Creating .NET Events
type EmptyCoffeeCupDelegate = delegate of obj * EventArgs -> unit

type EventfulCoffeeCup(amount : float<ml>) =
    let mutable m_amountLeft = amount
    let m_emptyCupEvent = new Event<EmptyCoffeeCupDelegate, EventArgs>()

    member this.Drink(amount) =
        printfn "Drinking %.1f..." (float amount)
        m_amountLeft <- max (m_amountLeft - amount) 0.0<ml>
        if m_amountLeft <= 0.0<ml> then
            m_emptyCupEvent.Trigger(this, new EventArgs())

    member this.Refil(amountAdded) =
        printfn "Coffee Cup refilled with %.1f" (float amountAdded)
        m_amountLeft <- m_amountLeft + amountAdded

    [<CLIEvent>]
    member this.EmptyCup = m_emptyCupEvent.Publish

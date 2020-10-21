module StudyFSharp.UnderstandingClasses

open System
open System.Collections.Generic
open System.IO

// 显式构造
type Point =
    val m_x : float
    val m_y : float
    
    // 两个参数的构造方法
    new (x, y) = { m_x = x; m_y = y }
    
    // 无参数构造
    new () = { m_x = 0.0; m_y = 0.0 }
    
    member this.Length =
        let sqr x = x * x
        sqrt <| sqr this.m_x + sqr this.m_y

type Point2 =
    val m_x : float
    val m_y : float
    
    // Parse a string, e.g. "1.0, 2.0"
    new (text : string) as this =
        // Do any required pre processing
        if text = null then
            raise <| ArgumentException("text")
            
        let parts = text.Split([| ',' |])
        let (successX, x) = Double.TryParse(parts.[0])
        let (successY, y) = Double.TryParse(parts.[1])
        if not successX || not successY then
            raise <| ArgumentException("text")

        // Initialize class fields
        { m_x = x; m_y = y }
        
        then
            // Do any post processing
            printfn
                "Initialized to [%f, %f]"
                this.m_x
                this.m_y

// 隐式构造
type Point3(x : float, y : float) =
    
    let length =
        let sqr x = x * x
        sqrt <| sqr x + sqr y
    do printfn "Initialized to [%f, %f]" x y
    
    member this.X = x
    member this.Y = y
    member this.Length = length
    
    // Define custom constructors, these must
    // call the 'main' constructor
    new() = new Point3(0.0, 0.0)
    
    // Define a second constructor.
    new(text : string) =
        if text = null then
            raise <| new ArgumentException("text")

        let parts = text.Split([| ',' |])
        let (successX, x) = Double.TryParse(parts.[0])
        let (successY, y) = Double.TryParse(parts.[1])
        if not successX || not successY then
            raise <| new ArgumentException("text")
        
        // Calls the primary constructor
        new Point3(x, y)

// 泛型类
type Arrayify<'a>(x : 'a) =
    member this.EmptyArray : 'a[] = [| |]
    member this.ArraySize1 : 'a[] = [| x |]
    member this.ArraySize2 : 'a[] = [| x; x |]
    member this.ArraySize3 : 'a[] = [| x; x; x |]

// 泛型联合
type GenDU<'a> =
    | Tag1 of 'a
    | Tag2 of string * 'a list

// 泛型记录
type GenRec<'a, 'b> = { Field1 : 'a; Field2 : 'b }

// Naming the this-pointer
type Circle =
    val m_radius : float
    
    new(r) = { m_radius = r }

    member foo.Radius = foo.m_radius
    member bar.Area = Math.PI * bar.Radius * bar.Radius

// 属性
type FancyMachine() =
    
    let mutable m_widget = Object()
    
    member this.GetWidget() = m_widget
    member this.SetWidget(newWidget) =
        m_widget <- newWidget

// 使用get、set关键字创建属性
[<Measure>]
type ml

type WaterBottle() =
    let mutable m_amount = 0.0<ml>
    
    // Read-only property
    member this.Empty = (m_amount = 0.0<ml>)

    // Read-write property
    member this.Amount with get () = m_amount
                        and set newAmt = m_amount <- newAmt

// 自动属性
type WaterBottle2() =
    member this.Empty = (this.Amount = 0.0<ml>)

    member val Amount = 0.0<ml> with get, set

// 定义类方法
type Television =
    
    val mutable m_channel : int
    val mutable m_turnedOn : bool
    
    new() = { m_channel = 3; m_turnedOn = true }
    
    member this.TurnOn() =
        printfn "Turning on..."
        this.m_turnedOn <- true
        
    member this.TurnOff() =
        printfn "Turning off..."
        this.m_turnedOn <- false

    member this.ChangeChannel(newChannel : int) =
        if this.m_turnedOn = false then
            failwith "Cannot change channel, the TV is not on."

        printfn "Changing channel to %d..." newChannel
        this.m_channel <- newChannel

    member this.CurrentChannel = this.m_channel

// Tuple参数
type Adder() =
    // Curried method arguments
    member this.AddTwoParams x y = x + y
    // Normal arguments
    member this.AddTwoTupledParams(x, y) = x + y

// 定义静态方法
type SomeClass() =
    static member StaticMember() = 5

// 定义静态字段
type RareType() =
    
    static let mutable m_numLeft = 2
    
    // 此处逻辑可以限定此类的实例化次数
    do
        if m_numLeft <= 0 then
            failwith "No more left!"
        m_numLeft <- m_numLeft - 1
        printfn "Initialized a rare type, only %d left!" m_numLeft

    static member NumLeft = m_numLeft

// 方法重载
type BitCounter =
    
    static member CountBits(x : int16) =
        let mutable x' = x
        let mutable numBits = 0
        for i = 0 to 15 do
            numBits <- numBits + int(x' &&& 1s)
            x' <- x' >>> 1
        numBits
        
    static member CountBits(x : int) =
        let mutable x' = x
        let mutable numBits = 0
        for i = 0 to 31 do
            numBits <- numBits + int(x' &&& 1)
            x' <- x' >>> 1
        numBits

    static member CountBits(x : int64) =
        let mutable x' = x
        let mutable numBits = 0
        for i = 0 to 63 do
            numBits <- numBits + int(x' &&& 1L)
            x' <- x' >>> 1
        numBits

// 访问修饰符, F#中只有三种:public, private, internal
type internal Ruby private(shininess, carats) =
    
    let mutable m_size = carats
    let mutable m_shininess = shininess
    
    member this.Polish() =
        this.Size <- this.Size - 0.1
        m_shininess <- m_shininess + 0.1

    member public this.Size with get () = m_size
    member private this.Size with set newSize = m_size <- newSize

    member this.Shininess = m_shininess

    public new() =
        let rng = Random()
        let s = float (rng.Next() % 100) * 0.01
        let c = float (rng.Next() % 16) + 0.1
        new Ruby(s, c)
    
    public new(carats) =
        let rng = Random()
        let s = float (rng.Next() % 100) * 0.01
        new Ruby(s, carats)

// 模块访问修饰符
module Logger =
    
    let mutable private m_filesToWriteTo = List<string>()
    
    let AddLogFile(filePath) = m_filesToWriteTo.Add(filePath)
    
    let LogMessage(message : string) =
        for logFile in m_filesToWriteTo do
            use file = new StreamWriter(logFile, true)
            file.WriteLine(message)
            file.Close()

// 继承
type BaseClass =
    val m_field1 : int
    
    new(x) = { m_field1 = x }
    member this.Field1 = this.m_field1

type ImplicitDerived(field1, field2) =
    inherit BaseClass(field1)
    
    let m_field2 : int = field2
    
    member this.Field2 = m_field2

type ExplicitDerived =
    inherit BaseClass
    
    val m_field2 : int
    
    new(field1, field2) =
        {
            inherit BaseClass(field1)
            m_field2 = field2
        }

    member this.Field2 = this.m_field2

// 方法重写
type Sandwich() =
    abstract Ingredients : string list
    default this.Ingredients = []
    
    abstract Calories : int
    default this.Calories = 0

type BLTSandwich() =
    inherit Sandwich()
    override this.Ingredients = ["Bacon"; "Lettuce"; "Tomato"]
    override this.Calories = 330

type TurkeySwissSandwich() =
    inherit Sandwich()
    override this.Ingredients = ["Turkey"; "Swiss"]
    override this.Calories = 330

// 使用base关键字
type BLTWithPickleSandwich() =
    inherit BLTSandwich()
    override this.Ingredients = "Pickles" :: base.Ingredients
    override this.Calories = 50 + base.Calories

// 定义抽象类
[<AbstractClass>]
type Bar() =
    member this.Alpha() = true
    abstract member Bravo : unit -> bool

// 定义密封类
[<Sealed>]
type Foo() =
    member this.Alpha() = true



// 与C#不同的是, 在F#中类不会自带隐式无参数构造方法
// 也就是说, 如果不为类定义构造方法, 则无法创建类实例
let p1 = new Point(1.0, 1.0)
let p2 = new Point()
let p3 = new Point2("8.0, 2.0")

let arrayifyTuple = new Arrayify<int * int>( (10, 27) )
printfn "result: %A" arrayifyTuple.ArraySize3

let inferred = new Arrayify<_>( "a string" )
printfn "result: %A" inferred.ArraySize2

printfn "result: %A" <| Tag2("Primary Colors", [ 'R'; 'G'; 'B' ])

printfn "result: %A" <| { Field1 = "Blue"; Field2 = 'C' }

// 使用get、set关键字
let bottle = new WaterBottle()
printfn "result: %A" bottle.Empty
bottle.Amount <- 1000.0<ml>
printfn "result: %A" bottle.Empty

// 使用自动属性
let bottle2 = new WaterBottle2()
printfn "result: %A" bottle2.Empty
bottle2.Amount <- 1000.0<ml>
printfn "result: %A" bottle2.Empty

let adder = new Adder()
let add10 = adder.AddTwoParams 10
printfn "add10: %A" <| add10 20
printfn "AddTwoTupledParams: %A" <| adder.AddTwoTupledParams(1, 2)

let arr = Array.init 2 (fun x -> new RareType() )
printfn "arr: %A" arr

// 类型转换
[<AbstractClass>]
type Animal() =
    abstract member Legs : int
    
[<AbstractClass>]
type Dog() =
    inherit Animal()
    abstract member Description : string
    override this.Legs = 4
    
type Pomeranian() =
    inherit Dog()
    override this.Description = "Furry"

let steve = Pomeranian()
let steveAsDog = steve :> Dog
let steveAsAnimal = steve :> Animal
let steveAsObject = steve :> obj

let steveAsDog2 = steveAsObject :?> Dog
//    let _ = steveAsObject :?> string

// 针对类型的模式匹配
let whatIs (x : obj) =
    match x with
    | :? string as s -> printfn "x is a string \"%s\"" s
    | :? int as i -> printfn "x is an int %d" i
    | :? list<int> as l -> printfn "x is an int list '%A'" l
    | _ -> printfn "x is a '%s'" <| x.GetType().Name

whatIs [1 .. 5]
whatIs "Rosebud"
whatIs <| System.IO.FileInfo(@"/Users/Kanbaru/.base_profile")

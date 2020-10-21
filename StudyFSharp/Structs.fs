module StudyFSharp.Structs

// 结构体
// * 与C#相同的是, F#的结构体在堆上分配内存
// * 结构体有默认的无参构造方法
// * 结构体不能被继承
// * 结构体不能覆盖默认构造方法
// * 结构体不能使用let绑定

// 定义结构体的两种方式
[<Struct>]
type StructPoint(x : int, y : int) =
    member this.X = x
    member this.Y = y

type StructRect(top : int, bottom : int, left : int, right : int) =
    struct
        member this.Top = top
        member this.Bottom = bottom
        member this.Left = left
        member this.Right = right
        override this.ToString() =
            sprintf "[%d, %d, %d, %d]" top bottom left right
    end

// Define two different struct values
let x = new StructPoint(6, 20)
let y = new StructPoint(6, 20)

// Struct for describing a book
[<Struct>]
type BookStruct(title : string, pages : int) =
    member this.Title = title
    member this.Pages = pages
    override this.ToString() =
        sprintf "Title: %A, Pages: %d" this.Title this.Pages

// 使用参数构造
let book1 = new BookStruct("Philosopher's Stone", 309)

// 使用系统默认无参构造
let namelessBook = new BookStruct()

// Define a struct with mutable fields
[<Struct>]
type MPoint =
    val mutable X : int
    val mutable Y : int
    override this.ToString() =
        sprintf "{%d, %d}" this.X this.Y

let mutable pt = new MPoint()
pt.X <- 10
pt.Y <- 7

let nonMutableStruct = new MPoint()
// nonMutableStruct.X <- 10 // ERROR: Update a non-mutable struct's field

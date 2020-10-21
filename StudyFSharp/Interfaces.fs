module StudyFSharp.Interfaces

// 定义隐式接口
type IDoStuff =
    abstract DoStuff : unit -> unit

// 显式定义接口
type IDoStuffToo =
    interface
        abstract member DoStuff : unit -> unit
    end

// 接口继承
type IDoMoreStuff =
    inherit IDoStuff
    
    abstract DoMoreStuff : unit -> unit

// 实现接口
type Bar() =
    interface IDoStuff with
        override this.DoStuff() = printfn "Stuff getting done..."
        
    interface IDoMoreStuff with
        override this.DoMoreStuff() = printfn "More stuff getting done..."

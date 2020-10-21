module StudyFSharp.FunctionComposition

open System
open System.IO

// 函数组合
let sizeOfFolder folder =
    
    // Get all files under the path
    let filesInFolder : string[] =
        Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
    
    let fileInfos : FileInfo[] =
        Array.map
            (fun (file : string) -> new FileInfo(file))
            filesInFolder
    
    let fileSizes : int64[] =
        Array.map
            (fun (info : FileInfo) -> info.Length)
            fileInfos
    
    let totalSize = Array.sum fileSizes
    
    totalSize
    
printfn "result %A" (sizeOfFolder (Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))

// 使用管道运算符简化以上代码
// 但是会使代码变得难以调试
let sizeOfFolderPiped folder =
    
    let getFiles path =
        Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
        
    let totalSize =
        folder
        |> getFiles
        |> Array.map (fun file -> new FileInfo(file))
        |> Array.map (fun info -> info.Length)
        |> Array.sum
        
    totalSize
    
printfn "result %A" (sizeOfFolderPiped (Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))

// 如果编译器不知道值的类型，则我们无法访问值的属性/方法
//List.iter (fun s -> printfn "s has length %d" s.Length) ["Pipe"; "forward"]

// 使用管道运算符可以帮助编译器推断类型
["Pipe"; "forward"] |> List.iter (fun s -> printfn "%s length: %d" s s.Length)

// 函数合成运算符可以省略参数占位
let sizeOfFolderComposed (*No Parameters!*) =
    
    let getFiles folder =
        Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
        
    getFiles
    >> Array.map (fun file -> new FileInfo(file))
    >> Array.map (fun info -> info.Length)
    >> Array.sum

printfn "result %A" (sizeOfFolderComposed (Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))

// 另一个使用合成运算符的例子
let square x = x * x
let toString x = x.ToString()
let strLen (x : string) = x.Length
let lenOfSquare = square >> toString >> strLen

printfn "result %A" (lenOfSquare 128)

// 反向管道运算符
printfn "The result of sprintf is %s" (sprintf "(%d, %d)" 1 2)
printfn "The result of sprintf is %s" <| sprintf "(%d, %d)" 1 2

// 反向合成运算符
//let square x = x * x
let negate x = -x

// 反向合成运算符会改变顺序
printfn "result %A" <| (square >> negate) 10
printfn "result %A" <| (square << negate) 10

// 反向合成运算符也可以改变阅读代码的方式
printfn "result %A"
<| ([ [1]; []; [4;5;6]; [3;4]; []; []; []; [9] ]
    |> List.filter (not << List.isEmpty))

printfn "result %A"
<| ([ [1]; []; [4;5;6]; [3;4]; []; []; []; [9] ]
    |> List.filter (fun l -> not (List.isEmpty l) ))

// 适当的使用|>, <|, >>, <<运算符可以使代码更容易理解
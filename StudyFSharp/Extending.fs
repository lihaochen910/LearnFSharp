namespace FSCollectionExtensions

open System
open System.Collections.Generic

//module StudyFSharp.Extending

// 扩展方法
// * 扩展方法无法访问类中private, internal成员
(*
type System.Int32 with
    member this.ToHexString() = sprintf "0x%x" this
*)

// 扩展模块
module List =
    
    /// Skips the first n elements of the list
    let rec skip n list =
        match n, list with
        | _, [] -> []
        | 0, list -> list
        | n, hd :: tl -> skip (n - 1) tl
        
module Seq =
    
    /// Reverse the elements in the sequence
    let rec rev (s : seq<'a>) =
        let stack = new Stack<'a>()
        s |> Seq.iter stack.Push
        seq {
            while stack.Count > 0 do
            yield stack.Pop()
        }
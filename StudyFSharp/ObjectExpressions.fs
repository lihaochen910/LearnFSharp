module StudyFSharp.ObjectExpressions

open System.Collections.Generic

// 接口的对象表达式
type Person =
    { First : string; Last : string }
    override this.ToString() = sprintf "%s, %s" this.Last this.First

[<AbstractClass>]
type Sandwich() =
    abstract Ingredients : string list
    abstract Calories : int
    
// 接口的对象表达式
let people =
    List<_>(
        [|
            { First = "Jomo"; Last = "Fisher" }
            { First = "Brian"; Last = "McNamara" }
            { First = "Joe"; Last = "Pamer" }
        |] )

let printPeople () =
    Seq.iter (fun person -> printfn "\t %s" (person.ToString())) people

printfn "Initial ordering:"
printPeople()

// Sort people by first name
people.Sort(
   {
       new IComparer<Person> with
           member this.Compare(l, r) =
               if l.First > r.First then 1
               elif l.First = r.First then 0
               else -1
   } )

printfn "After sorting by first name:"
printPeople()

// Sort people by last name
people.Sort(
   {
       new IComparer<Person> with
           member this.Compare(l, r) =
               if l.Last > r.Last then 1
               elif l.Last = r.Last then 0
               else -1
   } )

printfn "After sorting by first name:"
printPeople()

// 派生类的对象表达式, 无需显式声明类型(匿名类)
let lunch =
    {
        new Sandwich() with
            member this.Ingredients = ["Peanutbutter"; "Jelly"]
            member this.Calories = 400
    }

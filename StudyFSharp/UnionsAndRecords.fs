module StudyFSharp.UnionsAndRecords

open System
open System.Collections.Generic

type Suit =
    | Heart
    | Diamond
    | Spade
    | Club

type PlayingCard =
    | Ace of Suit
    | King of Suit
    | Queen of Suit
    | Jack of Suit
    | ValueCard of int * Suit
    
    member this.Value =
        match this with
        | Ace(_) -> 11
        | King(_) | Queen(_) | Jack(_) -> 10
        | ValueCard(x, _) when x <= 10 && x >= 2
            -> x
        | ValueCard(_) -> failwith "Card has an invalid value!"

type Number = Odd | Even

type Statement =
    | Print of string
    | Sequence of Statement * Statement
    | IfStmt of Expression * Statement * Statement
    
and Expression =
    | Interger of int
    | LessThan of Expression * Expression
    | GreaterThan of Expression * Expression

type BinaryTree =
    | Node of int * BinaryTree * BinaryTree
    | Empty

type Employee =
    | Manager of string * Employee list
    | Worker of string

type Person =
    | Person of string * string * int

type PersonRec =
    {First: string
     Last: string
     Age: int;}

type Car =
    {
        Make : string
        Model: string
        Year : int
    }

type Point = { X : float; Y : float }
type Vector3 =
    { X : float; Y : float; Z : float }
    member this.Lenth =
        sqrt <| this.X ** 2.0 + this.Y ** 2.0 + this.Z ** 2.0

// 生成扑克牌
let deckOfCards =
    [
        for suit in [ Spade; Club; Heart; Diamond ] do
            yield Ace(suit)
            yield King(suit)
            yield Queen(suit)
            yield Jack(suit)
            for value in 2 .. 10 do
                yield ValueCard(value, suit)
    ]
    
printfn "deckOfCards %A" deckOfCards

let rec printInOrder tree =
    match tree with
    | Node (data, left, right)
        -> printInOrder left
           printfn "Node %d" data
           printInOrder right
    | Empty -> ()

let binTree =
    Node(2,
         Node(1, Empty, Empty),
         Node(4,
              Node(3, Empty, Empty),
              Node(5, Empty, Empty)
        )
    )
    
printInOrder binTree

let describeHoleCards cards =
    match cards with
    | []
    | [_]
        -> failwith "Too few cards."
    | cards when List.length cards > 2
        -> failwith "Too many cards."
    | [ Ace(_); Ace(_) ] -> "Pocket Rockets"
    | [ King(_); King(_) ] -> "Cowboys"
    | [ ValueCard(2, _); ValueCard(2, _) ] -> "Ducks"
    | [ Queen(_); Queen(_) ]
    | [ Jack(_); Jack(_) ]
        -> "Pair of face cards"
    | [ ValueCard(x, _); ValueCard(y, _) ] when x = y
        -> "A Pair"
    | [ first; second ]
        -> sprintf "Two cards: %A and %A" first second

let rec printOrganization worker =
    match worker with
    | Worker(name) -> printfn "Employee %s" name
    
    // Manager with a worker list with one element
    | Manager(managerName, [ Worker(employeeName) ])
        -> printfn "Manager %s with Worker %s" managerName employeeName

    // Manager with a worker list of two elements
    | Manager(managerName, [ Worker(employee1); Worker(employee2) ])
        -> printfn
               "Manager %s with two workers %s and %s"
               managerName employee1 employee2

    // Manager with a list of workers
    | Manager(managerName, workers)
        -> printfn "Manager %s with workers..." managerName
           workers |> List.iter printOrganization

let company = Manager("Tom", [ Worker("Pam"); Worker("Stuart") ])
printOrganization company

let highCard = Ace(Spade)
let highCardValue = highCard.Value

let steve = Person("Steve", "Holt", 17)
let gob = Person("Bluth", "George Oscar", 36)

// 构造一个记录
let steveRec = { First = "Steve"; Last = "Holt"; Age = 17 }

printfn "%s is %d years old" steveRec.First steveRec.Age

// 使用with关键字克隆记录
let thisYear's = { Make = "FSharp"; Model = "Luxury Sedan"; Year = 2012 }
let nextYear's = { thisYear's with Year = 2013 }
let sameNextYear's =
    {
        Make = thisYear's.Make
        Model = thisYear's.Model
        Year = 2013
    }

//    let allCoups =
//        allNewCars
//        |> List.filter
//            (function
//                | { Model = "Coup" } -> true
//                | _ -> false)

let distance (pt1 : Point) (pt2 : Point) =
    let square x = x * x
    sqrt <| square(pt1.X - pt2.X) + square(pt1.Y - pt2.Y)

printfn "distance: %f" <| distance {X = 0.0; Y = 0.0} {X = 10.0; Y = 10.0}

let origin = { Point.X = 0.0; Point.Y = 0.0 }

let v = { X = 10.0; Y = 20.0; Z = 30.0 }

printfn "length: %f" <| v.Lenth
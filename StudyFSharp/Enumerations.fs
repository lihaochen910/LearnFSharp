module StudyFSharp.Enumerations

// 创建枚举
type ChessPiece =
    | Empty = 0
    | Pawn = 1
    | Knight = 3
    | Bishop = 4
    | Rook = 5
    | Queen = 8
    | King = 1000000

let createChessBoard() =
    let board = Array2D.init 8 8 (fun _ _ -> ChessPiece.Empty)

    // Place pawns
    for i = 0 to 7 do
        board.[1, i] <- ChessPiece.Pawn
        board.[6, i] <- enum<ChessPiece> (-1 * int ChessPiece.Pawn)
        
    // Place black pieces in order
    [| ChessPiece.Rook; ChessPiece.Knight; ChessPiece.Bishop; ChessPiece.Queen;
    ChessPiece.King; ChessPiece.Bishop; ChessPiece.Knight; ChessPiece.Rook |]
    |> Array.iteri(fun idx piece -> board.[0,idx] <- piece)
    
    // Place white pieces in order
    [| ChessPiece.Rook; ChessPiece.Knight; ChessPiece.Bishop; ChessPiece.King;
    ChessPiece.Queen; ChessPiece.Bishop; ChessPiece.Knight; ChessPiece.Rook |]
    |> Array.iteri(fun idx piece ->
        board.[7,idx] <- enum<ChessPiece> (-1 * int piece))
    
    // Return the board
    board
    
// 枚举的模式匹配
let isPawn piece =
    match piece with
    | ChessPiece.Pawn
        -> true
    | _ -> false

// 枚举转换
let invalidPiece = enum<ChessPiece>(42)
let validPiece = enum<ChessPiece>(1)
let materialvalueOfQueen = int ChessPiece.Queen

printfn "invalidPiece: %A" invalidPiece
printfn "validPiece: %A" validPiece

// 检查枚举值有效性
printfn "isDefined: %A" <| System.Enum.IsDefined(typeof<ChessPiece>, int ChessPiece.Bishop)
printfn "isDefined: %A" <| System.Enum.IsDefined(typeof<ChessPiece>, -711)

// Enumeration of flag values
type FlagsEnum =
    | OptionA = 0b0001
    | OptionB = 0b0010
    | OptionC = 0b0100
    | OptionD = 0b1000

let isFlagSet (enum : FlagsEnum) (flag : FlagsEnum) =
    let flagName = System.Enum.GetName(typeof<FlagsEnum>, flag)
    if enum &&& flag = flag then
        printfn "Flag [%s] is set." flagName
    else
        printfn "Flag [%s] is not set." flagName

let customFlags = FlagsEnum.OptionA ||| FlagsEnum.OptionC

isFlagSet customFlags FlagsEnum.OptionA
isFlagSet customFlags FlagsEnum.OptionB
isFlagSet customFlags FlagsEnum.OptionC
isFlagSet customFlags FlagsEnum.OptionD

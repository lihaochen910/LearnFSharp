module StudyFSharp.OrganizingFSharpCode

open System

module Utilities

// 如果要从模块外部访问这个值
// use: Utilities.x
let x = 1

module ConversionUtils =

    // Utilities.ConversionUtils.intToString
    let intToString (x : int) = x.ToString()
    
    module ConvertBase =
    
        // Utilities.ConversionUtils.ConvertBase.convertToHex
        let convertToHex x = sprintf "%x" x
        
        // Utilities.ConversionUtils.ConvertBase.convertToOct
        let convertToOct x = sprintf "%o" x

module DataTypes =

    // Utilities.DataTypes.Point
    type Point = Point of float * float * float


// 命名空间
namespace PlayingCards

// PlayingCards.Suit
type Suit =
    | Spade
    | Club
    | Diamond
    | Heart

// PlayingCards.PlayingCard
type PlayingCard =
    | Ace
    | King
    | Queen
    | Jack
    | ValueCard of int * Suit

namespace PlayingCards.Poker

// PlayingCards.Poker.PokerPlayer
type PokerPlayer = { Name : string; Money : int; Position : int }

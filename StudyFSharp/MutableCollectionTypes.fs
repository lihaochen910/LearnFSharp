module StudyFSharp.MutableCollectionTypes

open System
open System.Collections.Generic

[<Measure>]
type amu

type Atom = { Name : string; Weight : float<amu> }


// List<'T>
let planets = List<string>()

planets.Add("Mercury")
planets.Add("Venus")
planets.Add("Earth")
planets.Add("Mars")

printfn "Count: %d" planets.Count

planets.AddRange( [| "Jupiter"; "Saturn"; "Uranus"; "Neptune"; "Pluto" |] )

planets.Remove("Pluto")

// Dictionary<'K,'V>
let periodicTable = Dictionary<string, Atom>()

periodicTable.Add( "H", { Name = "Hydrogen"; Weight = 1.0079<amu> } )
periodicTable.Add( "He", { Name = "Helium"; Weight = 4.0026<amu> } )
periodicTable.Add( "Li", { Name = "Lithium"; Weight = 6.9410<amu> } )
periodicTable.Add( "Be", { Name = "Beryllium"; Weight = 9.0122<amu> } )
periodicTable.Add( "B", { Name = "Boron"; Weight = 10.811<amu> } )

// 查找一个元素
let printElement name =
    
    if periodicTable.ContainsKey(name) then
        let atom = periodicTable.[name]
        printfn
            "Atom with symbol with '%s' has weight %A."
            atom.Name atom.Weight
    else
        printfn "Error. No atom with name '%s' found." name

let printElement2 name =
    let (found, atom) = periodicTable.TryGetValue(name)
    if found then
        printfn
            "Atom with symbol with '%s' has weight %A."
            atom.Name atom.Weight
    else
        printfn "Error. No atom with name '%s' found." name

// HashSet<'T>
let bestPicture = new HashSet<string>()
bestPicture.Add("The Artist")
bestPicture.Add("The King's Speech")
bestPicture.Add("The Hurt Locker")
bestPicture.Add("Slumdog Milionaire")
bestPicture.Add("No Country for Old Men")
bestPicture.Add("The Departed")

if bestPicture.Contains("Manos: The Hands of Fate") then
    printfn "Sweet..."

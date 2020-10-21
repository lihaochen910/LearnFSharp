module StudyFSharp.LoopingConstructs

type Pet =
    | Cat of string * int // Name, Lives
    | Dog of string    // Name

// while循环
let mutable i = 0
while i < 5 do
    i <- i + 1
    printfn "i = %d" i

// for循环
for i = 1 to 5 do
    printfn "%d" i

for i = 5 downto 1 do
    printfn "%d" i

for i in [1 .. 5] do
    printfn "%d" i

let famousPets = [ Dog("Lassie"); Cat("Felix", 9); Dog("Rin Tin Tin") ]

for Dog(name) in famousPets do
    printfn "%s was a famous dog." name

// F#中没有break和continue, 如果想在循环中途退出, 则考虑使用while循环

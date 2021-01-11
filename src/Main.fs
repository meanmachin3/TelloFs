open Tello
open System

Tello.setIPPort "192.168.10.1" 8889
// |> Async.Parallel
// |> Async.Ignore
// |> Async.RunSynchronously

let mutable loop = true
while loop do
    let mutable key: ConsoleKeyInfo = Console.ReadKey()
    while key.Key <> ConsoleKey.Q do
    printfn "Pressed %c" key.KeyChar
    match key.Key with
    | ConsoleKey.T ->
        printfn "Taking off"
        Tello.takeoff |> ignore
    | ConsoleKey.Enter->
        printfn "Initialize"
        Tello.init |> ignore
    | ConsoleKey.L ->
        printfn "Land"
        Tello.comeDown |> ignore
    | ConsoleKey.RightArrow ->
        printfn "Right"
        Tello.moveRight "10" |> ignore
    | ConsoleKey.LeftArrow ->
        printfn "Left"
        Tello.moveLeft "10" |> ignore
    | ConsoleKey.UpArrow ->
        Tello.moveUp "10" |> ignore
    | ConsoleKey.DownArrow ->
        Tello.moveUp "10" |> ignore
    | _ -> ()
    key <- Console.ReadKey()
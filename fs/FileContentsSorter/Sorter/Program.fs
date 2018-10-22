// Learn more about F# at http://fsharp.org

open System
open Core

type OptionalInt = I of int | O of option<int>;;




[<EntryPoint>]
let main argv =
    let s0 = seq { for i in 1 .. 3 do yield i * i } |> Seq.toList
    let s1 = seq { for i in 1 .. 2 do yield i + 1 } |> Seq.toList
    //res |> Seq.iter Console.WriteLine




    0 // return an integer exit code

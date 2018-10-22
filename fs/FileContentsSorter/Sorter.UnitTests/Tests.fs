module Tests

open System.Collections.Generic
open Xunit
open Core

[<Fact>]
let ``merge2 for 2 seq returns single ordered seq`` () =
    let mergedSequence = merge2 [1 ; 4 ; 5] [2 ; 4; 7]
    let expectedSequence = seq {
        yield 1 
        yield 2
        yield 4
        yield 4
        yield 5
        yield 7
    }
    Assert.Equal<IEnumerable<int>>(expectedSequence, mergedSequence)

[<Fact>]
let ``quasi performance test for merge2`` () =
    let len = 100
    let s1 = seq { for n in [1..len] do yield n }
    let s2 = seq { for n in [1..len] do yield n + n }

    let mergedSequence = merge2 s1 s2

    Assert.Equal(len * 2, mergedSequence |> Seq.length)
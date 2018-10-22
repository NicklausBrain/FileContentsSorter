module Tests

open System.Collections.Generic
open Xunit
open Core

[<Fact>]
let ``mergeSeq for 2 seq returns single ordered seq`` () =
    let mergedSequence = mergeSeq [1 ; 4 ; 5] [2 ; 4; 7]
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
let ``mergeLists for 2 seq returns single ordered list`` () =
    let mergedSequence = mergeLists [1 ; 4 ; 5] [2 ; 4; 7]
    let expectedSequence = seq {
        yield 1 
        yield 2
        yield 4
        yield 4
        yield 5
        yield 7
    }
    Assert.Equal<IEnumerable<int>>(expectedSequence, mergedSequence)

//--------------------------------------------------------------------

let len = 100
let s1 = seq { for n in [1..len] do yield n } |> Seq.toList
let s2 = seq { for n in [1..len] do yield n + 1 } |> Seq.toList

[<Fact>]
let ``quasi performance test for mergeLists`` () =
    let mergedSequence = mergeLists s1 s2
    Assert.Equal(len * 2, mergedSequence |> Seq.length)

[<Fact>]
let ``quasi performance test for mergeSeq`` () =
    let mergedSequence = mergeSeq s1 s2
    Assert.Equal(len * 2, mergedSequence |> Seq.length)
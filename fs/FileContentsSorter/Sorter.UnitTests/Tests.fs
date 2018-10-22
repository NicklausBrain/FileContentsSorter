module Tests

open System
open System.Collections.Generic
open Xunit
open Core

[<Fact>]
let ``merge for 3 seq returns single ordered seq`` () =
    let mergedSequence = merge [[1 ; 4 ; 5] ; [1] ; [2 ; 7]]
    let expectedSequence = seq {
        yield 1 
        yield 1
        yield 2
        yield 4
        yield 5
        yield 7
    }
    Assert.Equal<IEnumerable<int>>(expectedSequence, mergedSequence)

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
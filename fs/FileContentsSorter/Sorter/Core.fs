module Core

let rec merge2 (seqA:seq<'t>) (seqB:seq<'t>) :seq<'t> =
    let a = seqA |> Seq.tryHead
    let b = seqB |> Seq.tryHead
    match (a, b) with
        | (None, Some b) -> next b seqA (seqB |> Seq.tail)
        | (Some a, None) -> next a (seqA |> Seq.tail) seqB
        | (Some a, Some b) ->
            let m = min a b
            if m = a
            then next a (seqA |> Seq.tail) seqB
            else next b seqA (seqB |> Seq.tail)
        | (None, None) -> Seq.empty

and next value seqA seqB =
    seq {
        yield value
        for v in merge2 seqA seqB do
        yield v
    }

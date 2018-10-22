module Core

let rec merge (seqs: seq<seq<'t>>) =
    let orderedSeqs =
        seqs
            |> Seq.filter(fun і -> і |> Seq.tryHead <> None)
            |> Seq.sortBy(fun і ->  (і |> Seq.tryHead))
    if orderedSeqs |> Seq.isEmpty
    then
        Seq.empty
    else
        let minSeq = orderedSeqs |> Seq.head
        let otherSeqs = orderedSeqs |> Seq.tail
        if minSeq |> Seq.tryHead <> None
        then
            let minValue = minSeq |> Seq.head
            let minTail = minSeq |> Seq.tail
            let next = [minTail] |> Seq.append(otherSeqs)
            merge next |> Seq.append [minValue]
        else Seq.empty

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

let rec factSeq n =
    seq {
        if n <= 1
        then yield 1
        else
            yield n
            for i in factSeq (n - 1) do
                yield i
    }
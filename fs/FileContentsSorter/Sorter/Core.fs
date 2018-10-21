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
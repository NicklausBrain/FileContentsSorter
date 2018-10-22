module Core

let mergeLists seqA seqB =
    let rec mergeCont cont seqA seqB = 
        match seqA, seqB with
        | seqA, [] -> cont seqA
        | [], seqB -> cont seqB
        | hl::tl, hr::tr ->
            if hl < hr then mergeCont (fun acc -> cont(hl::acc)) tl seqB
            else mergeCont (fun acc -> cont(hr::acc)) seqA tr
    mergeCont (fun x -> x) seqA seqB

let rec mergeSeq (seqA:seq<'t>) (seqB:seq<'t>) :seq<'t> =
    let a = seqA |> Seq.tryHead
    let b = seqB |> Seq.tryHead
    seq {
        if a <> None && b <> None
        then
            if min a b <> b
            then
                yield Option.get a
                yield! mergeSeq (Seq.skip 1 seqA) seqB
            else
                yield Option.get b
                yield! mergeSeq seqA (Seq.skip 1 seqB)
         elif a <> None
         then yield! seqA
         elif b <> None
         then yield! seqB
    }

module IssuesFS.Test

open NUnit.Framework
open FsUnit
open IssuesFS.CLI
open FSharp.Data
open JsonExtensions

[<Test>]
let ``Help returned by option parsing with -h and --help options`` () =
    parseArgs [ "-h"; "anything"; ] |> should equal Help
    parseArgs [ "--help"; "anything"; ] |> should equal Help


[<Test>]
let ``three values returned if three given`` () =
    parseArgs [ "user"; "project"; "99"; ] |> should equal (Opts({ user = "user"; project = "project"; count = 99; }))

[<Test>]
let ``count is defaulted if two values given`` () =
    parseArgs [ "user"; "project"; ] |> should equal (Opts({ user = "user"; project = "project"; count = 4; }))

let fakeCreatedAtList values =
    values |> Seq.map (fun v -> $"{{\"created_at\": \"{v}\", \"other_data\": \"xxx\"}}" |> JsonValue.Parse) |>  Seq.toArray

[<Test>]
let ``sort descending orders the correct way`` () =
    let result = sortIntoDescendingOrder (fakeCreatedAtList["c"; "a"; "b"])
    let issues = Seq.map (fun i -> i?created_at.AsString()) result
    issues |> should equal ["c";"b";"a"]

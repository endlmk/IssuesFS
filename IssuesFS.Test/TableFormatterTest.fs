module TableFormatter.Test

open NUnit.Framework
open FsUnit
open IssuesFS.TableFormatter
open FSharp.Data
open JsonExtensions

let simpleTestData =[|
    JsonValue.Parse("""{ "c1": "r1 c1", "c2": "r1 c2", "c3": "r1 c3", "c4": "r1+++c4" }""");
    JsonValue.Parse("""{ "c1": "r2 c1", "c2": "r2 c2", "c3": "r2 c3", "c4": "r2 c4" }""");
    JsonValue.Parse("""{ "c1": "r3 c1", "c2": "r3 c2", "c3": "r3 c3", "c4": "r3 c4" }""");
    JsonValue.Parse("""{ "c1": "r4 c1", "c2": "r4++c2", "c3": "r4 c3", "c4": "r4 c4" }""");
|]

let header = [ "c1"; "c2"; "c4"; ]

let splitWithThreeColumns = 
    splitIntoColumns simpleTestData header

[<Test>]
let ``splitIntoColumns`` () =
    let result = splitWithThreeColumns
    Seq.head splitWithThreeColumns |> should equal (seq {"r1 c1"; "r2 c1"; "r3 c1"; "r4 c1";})
    Seq.last splitWithThreeColumns |> should equal (seq {"r1+++c4"; "r2 c4"; "r3 c4"; "r4 c4";})

[<Test>]
let ``columnWidths``  () =
    let widths = widthOf splitWithThreeColumns
    widths |> should equal (seq { 5; 6; 7; })

[<Test>]
let ``correct format field returned`` () =
    formattedField "aaa" 9 |> should equal "aaa      "

[<Test>]
let ``Output is correct`` () =
    let result = printTableForColumns simpleTestData header
    result |> should equal """c1    | c2     | c4     
------+--------+--------
r1 c1 | r1 c2  | r1+++c4
r2 c1 | r2 c2  | r2 c4  
r3 c1 | r3 c2  | r3 c4  
r4 c1 | r4++c2 | r4 c4  
"""
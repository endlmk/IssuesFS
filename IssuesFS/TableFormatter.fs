
namespace IssuesFS

module TableFormatter =
    open FSharp.Data
    open JsonExtensions

    let printable (j : JsonValue) =
        j.AsString()

    let splitIntoColumns (rows : JsonValue[]) headers =
        seq {for header in headers do
                seq {for row in rows -> printable (row.GetProperty(header)) }
        }

    let widthOf columns =
        seq { for column in columns -> column |> Seq.map String.length |> Seq.max }
     
    let formattedField (field : string) columnWidth =
        sprintf (PrintfFormat<string -> string, unit, string, string>(sprintf "%%-%ds" columnWidth)) field
        
    let separeator columnWidths =
        (columnWidths |> Seq.map (fun i -> String.replicate i "-") |> String.concat "-+-") + System.Environment.NewLine

    let putsOneLineInColumns (fields : seq<string>) (columnWidths : seq<int>)= 
        (Seq.zip fields columnWidths |> Seq.map (fun (s, w) -> formattedField s w) |> String.concat " | ") + System.Environment.NewLine

    let putsInColumns (rows : JsonValue[]) headers columnWidths =
        let rowStrs = seq { for row in rows do
                            seq { for header in headers -> printable (row.GetProperty(header)) } } 
        rowStrs |> Seq.map (fun fields -> putsOneLineInColumns fields columnWidths) |> String.concat ""

    let printTableForColumns (rows : JsonValue[]) headers =
        let dataByColumns = splitIntoColumns rows headers
        let columnWidths = Seq.zip (widthOf dataByColumns) (headers |> Seq.map String.length) |> Seq.map (fun (d, h) -> max d h)

        (putsOneLineInColumns headers columnWidths) + (separeator columnWidths) + (putsInColumns rows headers columnWidths) 
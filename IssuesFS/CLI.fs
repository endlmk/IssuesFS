// Learn more about F# at http://fsharp.org
namespace IssuesFS

module CLI = 

    open System
    open CommandLine
    open FSharp.Data
    open JsonExtensions

    let defaultCount = 4

    type Options = {
        [<Value(0, MetaName = "user", Required = true)>] user : string;
        [<Value(1, MetaName = "project", Required = true)>] project : string;
        [<Value(2, MetaName = "count", Default = 4)>] count : int;
    }

    type Args = Opts of Options 
              | Help 

    let parseArgs argv =
        let parser = new CommandLine.Parser(fun config -> config.HelpWriter <- null)
        let result = parser.ParseArguments<Options>(argv)
        match result with
        | :? Parsed<Options> as parsed -> Opts(parsed.Value) 
        | _ -> Help

    let decodeResponse (result : Result<JsonValue, JsonValue>) =
        match result with 
        | Ok(body) -> body.AsArray()
        | Error(error) -> (System.Console.WriteLine($"Error fetching from Github: {error?message}"); exit(0);)

    let sortIntoDescendingOrder (issues : JsonValue[]) =
        issues
        |> Seq.sortWith (fun i1 i2 -> if i1?created_at >= i2?created_at then -1 else 1)
    
    let last count list =
        list
        |> Seq.take count
        |> Seq.rev
        |> Seq.toArray
        
    let execute args =
        match args with
        | Opts({ user = u; project = p; count = c; }) 
            -> IssuesFS.GithubIssues.fetch u p
                |> decodeResponse
                |> sortIntoDescendingOrder
                |> last c
                |> fun issues -> IssuesFS.TableFormatter.printTableForColumns issues (seq{ "number"; "created_at"; "title"; })
        | Help 
            -> (System.Console.WriteLine($"usage: issues <user> <project> [ count | {defaultCount} ]"); exit(0);)

    [<EntryPoint>]
    let main argv =
        argv 
        |> parseArgs
        |> execute
        |> printfn "%s"
        0 // return an integer exit code






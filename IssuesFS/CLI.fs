// Learn more about F# at http://fsharp.org
namespace IssuesFS

module CLI = 

    open System
    open CommandLine
    
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

    [<EntryPoint>]
    let main argv =
        printfn "Hello World from F#!"
        0 // return an integer exit code






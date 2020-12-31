
namespace IssuesFS

module GithubIssues =
    open System
    open FSharp.Data
    open FSharp.Data.JsonExtensions

    let userAgent = [ "User-agent", "Elixir dave@pragprog.com" ]

    let issuesUrl user project =
        $"https://api.github.com/repos/{user}/{project}/issues"

    let parseResponseBody (responseBody : HttpResponseBody) = 
        match responseBody with
        | Text(body) -> JsonValue.Parse(body)
        | Binary(_) ->  (System.Console.WriteLine("Unexpected response body type: Binary"); exit(0);)
    
    let handleResponse (response : HttpResponse) = 
        let responseJson = response.Body |> parseResponseBody
        match response.StatusCode with
        | 200 -> Ok (responseJson)
        | _ -> Error (responseJson)

    let fetch user project =
        let request url = Http.Request(url, headers = userAgent, silentHttpErrors = true)
        issuesUrl user project
        |> request
        |> handleResponse

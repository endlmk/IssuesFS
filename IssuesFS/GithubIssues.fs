namespace ISsuesFS
module GithubIssues =

    open FSharp.Data

    let userAgent = [ "User-agent", "Elixir dave@pragprog.com" ]

    let issuesUrl user project =
        $"https://api.github.com/repos/{user}/{project}/issues"
    
    let handleResponse (response : HttpResponse) = 
        match response.StatusCode with
        | 200 -> Ok response.Body
        | _ -> Error response.Body
    
    let fetch user project =
        let request url = Http.Request(url, headers = userAgent)
        issuesUrl user project
        |> request
        |> handleResponse

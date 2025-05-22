namespace Presentation.Models;

public class RepoResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }

}
public class RepoResult<T> : RepoResult
{
    public T? Data { get; set; }
}

﻿namespace Presentation.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? Token { get; set; }

}
public class AuthResult<T> : AuthResult
{
    public T? Data { get; set; }
}

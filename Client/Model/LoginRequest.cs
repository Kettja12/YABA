﻿namespace Client.Model
{
    public class LoginRequest
    {
        required public string Username { get; set; }
        required public string Password { get; set; }    
    }
}
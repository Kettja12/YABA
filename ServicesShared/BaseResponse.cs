﻿namespace ServicesShared;
public class BaseResponse
{
    public int Status { get; set; }
    public string Message { get; set; } = null!;
}

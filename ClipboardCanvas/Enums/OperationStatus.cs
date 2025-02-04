﻿namespace ClipboardCanvas.Enums
{
    /// <summary>
    /// Contains all kinds of return statuses
    /// </summary>
    public enum OperationErrorCode : uint
    {
        Success = 0,
        UnknownFailed = 1,
        Cancelled = 2,
        AccessUnauthorized = 4,
        NotFound = 8,
        AlreadyExists = 16,
        NotAFolder = 32,
        NotAFile = 64,
        InProgress = 128,
        InvalidArgument = 256,
        InvalidOperation = 512
    }
}

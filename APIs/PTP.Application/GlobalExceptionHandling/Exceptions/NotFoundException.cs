﻿namespace PTP.Application.GlobalExceptionHandling.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string? message) : base(message)
        {
        }
    }
}

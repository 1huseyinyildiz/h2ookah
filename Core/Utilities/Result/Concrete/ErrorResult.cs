﻿namespace Core.Utilities.Result.Concrete
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false)
        {
        }

        public ErrorResult(string message) : base(false, message)
        {
        }

        public ErrorResult(string message, int code) : base(false, message, code)
        {
        }
    }
}

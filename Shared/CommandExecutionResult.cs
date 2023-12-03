﻿namespace Shared
{
    public class CommandExecutionResult
    {
        public string? ResultId { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
        public string? ErrorMessage { get; set; }
        public long? ListCount { get; set; }

        public int Code = 200;
    }
}
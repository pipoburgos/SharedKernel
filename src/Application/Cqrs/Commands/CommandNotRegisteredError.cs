using System;

namespace SharedKernel.Application.Cqrs.Commands
{
    /// <summary>
    /// Command not register exception
    /// </summary>
    public class CommandNotRegisteredError : Exception
    {
        /// <summary>
        /// Command not register exception
        /// </summary>
        /// <param name="command"></param>
        public CommandNotRegisteredError(string command) : base($"The command {command} has not a command handler associated") { }
    }
}

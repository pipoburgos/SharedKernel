using System;

namespace SharedKernel.Application.Cqrs.Commands
{
    /// <summary>
    ///  Exception thrown if the command handler is not registered in the dependency container
    /// </summary>
    public class CommandNotRegisteredError : Exception
    {
        /// <summary>
        ///  Exception thrown if the command handler is not registered in the dependency container
        /// </summary>
        /// <param name="command"></param>
        public CommandNotRegisteredError(string command) : base($"The command {command} has not a command handler associated") { }
    }
}

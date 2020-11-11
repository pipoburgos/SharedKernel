using System;

namespace SharedKernel.Application.Cqrs.Commands
{
    public class CommandNotRegisteredError : Exception
    {
        public CommandNotRegisteredError(string command) : base(
            $"The command {command} has not a command handler associated")
        {
        }
    }
}

using System;
using System.Linq;
using CommandSystem;
using Exiled.CustomRoles.API.Features;
using Exiled;
using Exiled.API;




namespace Mask096
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class isrpcmd : ICommand
    {
        public string Command => "RP";
        public string Description => "do this command to have mtf captains will spawn with a paper bag";
        public string[] Aliases => Array.Empty<string>();


        

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Config.isrp = true;

            response = null;
            return true;
        }
    }
}

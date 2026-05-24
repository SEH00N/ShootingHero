using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootingHero.Servers
{
    public struct GetServerCommandLineArguments
    {
        public string UUID;
        public int Port;
        public bool IsValid;

        public GetServerCommandLineArguments(string[] args)
        {
            UUID = "";
            Port = -1;
            IsValid = false;

            Dictionary<string, string> argsMap = args
                .Where(i => i.StartsWith("--"))
                .Select(i => i.Substring(2).Split('='))
                .Where(i => i.Length == 2)
                .ToDictionary(i => i[0], i => i[1], StringComparer.OrdinalIgnoreCase);

            if(argsMap.TryGetValue("uuid", out string uuid) == false)
            {
                IsValid = false;
                return;
            }

            if(argsMap.TryGetValue("port", out string port) == false)
            {
                IsValid = false;
                return;
            }

            if(int.TryParse(port, out int portAsInt) == false)
            {
                IsValid = false;
                return;
            }

            UUID = uuid;
            Port = portAsInt;
            IsValid = true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Orcabot.Types;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Sys = Orcabot.Types.StarSystem;
using System.IO;

namespace Orcabot.Helpers
{
    public static class JSONParser
    {
        /// <summary>
        /// Returns a Dictionary of Systems if Parsing is successful. Returns null upon failure.
        /// </summary>
        /// <param name="path"> The URI of the File that contains the Systems JSON</param>
        /// <returns>Null or a Dict<string,Sys></returns>
        static public bool TryGetSystemsFromJSON(string path, out Dictionary<string, Sys> sys, out Exception e) {
            sys = null;
            e = null;
            if (!File.Exists(path)) {
                e = new FileNotFoundException(path);
                return false;
            }





            try {
                var dataAsString = File.ReadAllText(path);
                sys = JsonConvert.DeserializeObject<Dictionary<string, Sys>>(dataAsString);
            }
            catch (Exception b) {
                e = b;
                return false;
            }
            return true;
        }
    }
}

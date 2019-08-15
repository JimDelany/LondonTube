using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonTubeConsole
{
    public static class DataStore
    {
        public static List<NodeModel> Nodes = new List<NodeModel>();
        public static Dictionary<string, List<NodeModel>> Stops = new Dictionary<string, List<NodeModel>>();
        //
        // dictionary of stop names vs next stops
        //public static Dictionary<string, List<string>> Mesh = new Dictionary<string, List<string>>();

        public static List<string> AllTheStops = new List<string>();

        public static void StoreStop(string stop)
        {
            if (AllTheStops.Contains(stop) == false)
                AllTheStops.Add(stop);
        }
    }
}

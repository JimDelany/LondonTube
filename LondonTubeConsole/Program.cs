//#define USETESTDATA
//#define ASTRAINSRUN
#define STARTREKMOD
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LondonTubeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(args[0]);
            string find = args[1];
#if USETESTDATA
            find = "a";
#endif
            LoadData();

            NodeModel root = DataStore.Nodes.FirstOrDefault(x => x.FromStation == find);

            //
            // we start with the first child so decrement the count to account for that.
            //
            //n--;
            DescendTree(n, root, new List<string>());

            Console.WriteLine("________________________________________________________________________");
            DataStore.AllTheStops.Sort();
            foreach (string s in DataStore.AllTheStops)
                Console.WriteLine(s);
        }

        /// <summary>
        /// Load the csv info into the tree and also make a list of links into and out of each node
        /// </summary>
        static void LoadData()
        {
#if USETESTDATA
            using (var reader = new StreamReader(@"d:\test.csv"))
#else
            using (var reader = new StreamReader(@"d:\lines.csv"))
#endif
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values.Length != 3)
                        throw new Exception("Invalid csv file format 3 columns required.");

                    string tubeName = values[0].Trim();
                    string fromStation = values[1].Trim();
                    string toStation = values[2].Trim();

                    NodeModel model = new NodeModel(tubeName, fromStation, toStation);
                    DataStore.Nodes.Add(model);

                }

                //
                // loop through the stored nodes and make a list of links which either point to this node or emilate from this node
                //
                foreach(NodeModel model in DataStore.Nodes)
                {
                    IEnumerable<NodeModel> nodes;

                    //
                    // store the nodes which reference this node
                    //
                    nodes = DataStore.Nodes.Where(x => x.ToStation == model.FromStation);
                    foreach (NodeModel m in nodes)
                        if (model.NodeTreeIn.Contains(m)==false)
                            model.NodeTreeIn.Add(m);
                    //
                    // store the links which are referencing other nodes from this node
                    //
                    nodes = DataStore.Nodes.Where(x => x.FromStation == model.ToStation);
                    foreach (NodeModel m in nodes)
                        if (model.NodeTreeOut.Contains(m) == false)
                            model.NodeTreeOut.Add(m);
                }
            }
        }

        /// <summary>
        /// descend through the links in this node to attempt to find the terminus nodes whic are stored in a list to be displayed later
        /// </summary>
        /// <param name="n">The current depth of the search left</param>
        /// <param name="model">The node to process</param>
        /// <param name="OldNodes">the stops already processed</param>
        public static void DescendTree(int n, NodeModel model, List<string> OldNodes)
        {
            //
            // if we are done then if this is a unique entry add it to the list and return
            //
            if (n-- == 0)
            {
                if (DataStore.AllTheStops.Contains(model.FromStation) == false)
                {

                    Console.WriteLine("\n\n");
                    foreach (string s in OldNodes)
                        Console.WriteLine(s);

                    DataStore.AllTheStops.Add(model.FromStation);
                    Console.WriteLine("Returning  " + model.FromStation);
                }
                return;
            }

            //
            // since some of the links go back we need to keep track of where we came from to avoid infinate loops
            //
            if (OldNodes.Contains(model.FromStation) == false)
                OldNodes.Add(model.FromStation);
            if (OldNodes.Contains(model.ToStation) == false)
                OldNodes.Add(model.ToStation);

            //
            // process the nodes which reference this node
            //
            foreach (NodeModel subNode in model.NodeTreeIn)
            {
                if (OldNodes.Contains(subNode.FromStation) == false)
                    DescendTree(n, subNode, OldNodes.ToList());
            }

            //
            // process the nodes which are referenced from this node
            //
            foreach (NodeModel subNode in model.NodeTreeOut)
                if (OldNodes.Contains(subNode.ToStation) == false)
                    DescendTree(n, subNode, OldNodes.ToList());

        }
    }
}
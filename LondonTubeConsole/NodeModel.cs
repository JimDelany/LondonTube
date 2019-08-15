using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonTubeConsole
{
    public class NodeModel
    {
        public NodeModel(string Name, string FromStation, string ToStation)
        {
            this.Name = Name;
            this.FromStation = FromStation;
            this.ToStation = ToStation;

        }
        private List<NodeModel>  _nodeTreeOut = new List<NodeModel>();
        private List<NodeModel> _nodeTreeIn = new List<NodeModel>();

        public string Name { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public List<NodeModel> NodeTreeIn { get { return _nodeTreeIn; } }
        public List<NodeModel> NodeTreeOut { get { return _nodeTreeOut; } }

        /// <summary>
        /// /having this tostring really helps debugging since the debugger will then display this string instead of the class.tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FromStation + "=>" + ToStation;
        }
    }
}

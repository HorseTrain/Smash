using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Model
{
    public class ModelEntry
    {
        public string Name { get; set; }
        public long SubIndex { get; set; }
        public string MaterialName { get; set; }

        public override string ToString()
        {
            return Name + " " + SubIndex + " " + MaterialName;
        }
    }
}

using SmashLabs.IO.Parsables.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Encoders.Mesh
{
    public class ExportableMeshObject
    {
        public string Name { get; set; }
        public int SubIndex { get; set; }
        public string ParentBoneName { get; set; }
        public MeshObjectData MeshData;
        public List<ExportableMeshAttribute> Attributes { get; set; }
        public ushort[] MeshPolygons { get; set; }
    }
}

using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.Tools.Encoders.Mesh;
using SmashLabs.Tools.Exporter;
using SmashLabs.Tools.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Encoders
{
    public class MeshCollectionBuilder : List<ExportableMeshObject>
    {
        public ByteBuffer VertexBuffer { get; set; } = new ByteBuffer();
        public ByteBuffer PolygonBuffer { get;set; } = new ByteBuffer();
        public ByteBuffer RigBuffer { get; set; } = new ByteBuffer();

        public HSEM BuildFile()
        {
            HSEM Out = new HSEM();

            ExportableBufferCollection Collection = new ExportableBufferCollection();

            Out.reader = new BufferReader(Collection.BuildBuffer().ToArray());

            return Out;
        }
    }
}

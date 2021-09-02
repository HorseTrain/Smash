using SmashLabs.IO.Parsables.Material.Enums;
using SmashLabs.Structs;
using SmashLabs.Tools.Exporter;
using SmashLabs.Tools.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material
{
    public class LTAM : IParsable
    {
        public MaterialEntry[] Entries { get; set; }

        public override void LoadData()
        {
            Entries = MaterialEntry.ParseEntries(reader);
        }

        public override unsafe byte[] GetData()
        {
            ExportableBufferCollection Out = new ExportableBufferCollection();

            ByteBuffer Header = Out.AddBuffer();

            ByteBuffer EntryBuffer = Out.AddBuffer();

            ByteBuffer ShaderAttributeBuffer = Out.AddBuffer();

            ByteBuffer StringOffsets = Out.AddBuffer();

            ByteBuffer DataBuffer = Out.AddBuffer();

            Header.AddObject(this.Header);
            Header.AddObject(Magic);

            Out.AddArrayPointer(Header,EntryBuffer,Entries.Length);

            foreach (MaterialEntry entry in Entries)
            {
                Out.AddStringWithPointer(entry.Label,EntryBuffer,DataBuffer);

                Out.AddArrayPointer(EntryBuffer,ShaderAttributeBuffer,entry.Attributes.Length);

                foreach (MaterialAttribute attribute in entry.Attributes)
                {
                    ShaderAttributeBuffer.AddObject(attribute.paramID);

                    if (attribute.DataType == ParamDataType.String)
                    {
                        Out.AddPointer(ShaderAttributeBuffer,StringOffsets);

                        Out.AddStringWithPointer((string)attribute.DataObject,StringOffsets,DataBuffer);
                    }
                    else
                    {
                        Out.AddPointer(ShaderAttributeBuffer, DataBuffer);

                        switch (attribute.DataType)
                        {
                            case ParamDataType.Float: DataBuffer.AddObject((float)attribute.DataObject); break;
                            case ParamDataType.Boolean: DataBuffer.AddObject(MaterialAttribute.GetIntFromBool((bool)attribute.DataObject)); break;
                            case ParamDataType.Vector4: DataBuffer.AddObject((Vector4)attribute.DataObject); break;
                            case ParamDataType.Sampler: DataBuffer.AddObject((Sampler)attribute.DataObject); break;
                            case ParamDataType.UvTransform: DataBuffer.AddObject((UVTransform)attribute.DataObject); break;
                            case ParamDataType.BlendState: DataBuffer.AddObject((BlendState)attribute.DataObject); break;
                            case ParamDataType.RasterizerState: DataBuffer.AddObject((RasterizerState)attribute.DataObject); break;
                        }
                    }

                    ShaderAttributeBuffer.AddObject(attribute.DataType);
                }

                Out.AddStringWithPointer(entry.ShaderLabel,EntryBuffer,DataBuffer);
            }

            return Out.FinalBuffer();
        }
    }
}

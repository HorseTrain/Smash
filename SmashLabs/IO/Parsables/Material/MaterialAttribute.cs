using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material
{
    public class MaterialEntry
    {
        public string Label { get; set; }
        public string ShaderLabel { get; set; }
        public MaterialAttribute[] Attributes { get; set; }

        public static MaterialEntry[] ParseEntries(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            MaterialEntry[] Out = new MaterialEntry[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseEntry(reader);
            }

            pointer.End();

            return Out;
        }

        public static MaterialEntry ParseEntry(BufferReader reader)
        {
            MaterialEntry Out = new MaterialEntry();

            Out.Label = reader.ReadStringOffset();

            Out.Attributes = MaterialAttribute.ParseMaterialAttributes(reader);

            Out.ShaderLabel = reader.ReadStringOffset();

            return Out;
        }
    }
}

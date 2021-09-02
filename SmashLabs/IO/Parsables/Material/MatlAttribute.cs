using SmashLabs.IO.Parsables.Material.Enums;
using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material
{
    public class MaterialAttribute
    {
        public ParamID paramID { get; set; }
        public SsbhOffset OffsetToData { get; set; }
        public ParamDataType DataType { get; set; }
        public string DataTypeString { get; set; }

        object dataObject;
        public object DataObject
        {
            get => dataObject;

            set
            {
                dataObject = value;
                DataTypeString = value.GetType().ToString();
            }
        }

        public static MaterialAttribute[] ParseMaterialAttributes(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            MaterialAttribute[] Out = new MaterialAttribute[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseMaterialAttribute(reader);
            }

            pointer.End();

            return Out;
        }

        public static MaterialAttribute ParseMaterialAttribute(BufferReader reader)
        {
            MaterialAttribute Out = new MaterialAttribute();

            Out.paramID = reader.ReadObject<ParamID>();
            Out.OffsetToData = reader.ReadOffset();
            Out.DataType = reader.ReadObject<ParamDataType>();

            long temp = reader.BufferLocation;
            Out.ReadDataObject(reader);
            reader.Seek(temp);

            return Out;
        }

        void ReadDataObject(BufferReader reader)
        {
            reader.Seek(OffsetToData);

            switch (DataType)
            {
                case ParamDataType.Float:
                    DataObject = reader.ReadSingle();
                    break;
                case ParamDataType.Boolean:
                    DataObject = reader.ReadInt() == 1; // should this just be > 0?
                    break;
                case ParamDataType.Vector4:
                    DataObject = reader.ReadObject<Vector4>();
                    break;
                case ParamDataType.String:
                    DataObject = reader.ReadStringOffset();
                    break;
                case ParamDataType.Sampler:
                    DataObject = reader.ReadObject<Sampler>();
                    break;
                case ParamDataType.UvTransform:
                    DataObject = reader.ReadObject<UVTransform>();
                    break;
                case ParamDataType.BlendState:
                    DataObject = reader.ReadObject<BlendState>();
                    break;
                case ParamDataType.RasterizerState:
                    DataObject = reader.ReadObject<RasterizerState>();
                    break;
            }
        }

        public static int GetIntFromBool(bool data)
        {
            if (data)
                return 1;
            else
                return 0;
        }
    }
}

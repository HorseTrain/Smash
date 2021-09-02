using SmashLabs.IO.Parsables.Skeleton;
using SmashLabs.Structs;
using SmashLabs.Tools.Exporter;
using SmashLabs.Tools.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Model
{
    public class LDOM : IParsable
    {
        public string Name { get; set; }
        public string SkeletonFileName { get; set; }
        public string[] MaterialFileNames { get; set; }
        public string UnknownFileName { get; set; } 
        public string MeshCollectionFileName { get; set; }
        public ModelEntry[] Entries { get; set; }

        public override void LoadData()
        {
            Name = reader.ReadStringOffset();
            SkeletonFileName = reader.ReadStringOffset();

            MaterialFileNames = reader.ReadStringArray();

            UnknownFileName = reader.ReadStringOffset();
            MeshCollectionFileName = reader.ReadStringOffset();

            BufferArrayPointer entrypointers = reader.ReadArrayPointer();
            Entries = new ModelEntry[entrypointers.ElementCount];

            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = ReadEntry();
            }
        }

        ModelEntry ReadEntry()
        {
            return new ModelEntry()
            {
                Name = reader.ReadStringOffset(),
                SubIndex = reader.ReadLong(),
                MaterialName = reader.ReadStringOffset()
            };
        }

        public unsafe override byte[] GetData()
        {
            ExportableBufferCollection Out = new ExportableBufferCollection();

            ByteBuffer Header = Out.AddBuffer();

            ByteBuffer MaterialNamePointers = Out.AddBuffer();

            ByteBuffer EntryDataBuffer = Out.AddBuffer();

            ByteBuffer StringBuffer = Out.AddBuffer();

            Header.AddObject(this.Header);
            Header.AddObject(Magic);

            Out.AddStringWithPointer(Name,Header, StringBuffer);
            Out.AddStringWithPointer(SkeletonFileName, Header, StringBuffer);

            Out.AddPointer(Header,MaterialNamePointers);
            Header.AddObject((long)MaterialFileNames.Length);

            foreach (string matentry in MaterialFileNames)
            {
                Out.AddStringWithPointer(matentry,MaterialNamePointers, StringBuffer);
            }

            Header.AddObject(0L);
            Out.AddStringWithPointer(MeshCollectionFileName,Header,StringBuffer);

            Out.AddPointer(Header, EntryDataBuffer);
            Header.AddObject((long)Entries.Length);

            int entryp = 0;

            foreach (ModelEntry entry in Entries)
            {
                Out.AddStringWithPointer(entry.Name,EntryDataBuffer,StringBuffer);

                EntryDataBuffer.AddObject(entry.SubIndex);

                Out.AddStringWithPointer(entry.MaterialName, EntryDataBuffer, StringBuffer,entryp != Entries.Length - 1);

                entryp++;
            }

            return Out.FinalBuffer();
        }
    }
}

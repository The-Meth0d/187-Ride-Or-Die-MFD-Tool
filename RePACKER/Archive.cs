using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace RePACKER
{
    public class Item
    {
        public uint Id;
        public uint Offset;
        public uint Size;
    }

    public class Archive
    {
        public List<Item> Items = new List<Item>();

        public FileStream fs;
        public BinaryReader br;
        public BinaryWriter bw;

        public string Filename;
        public string Format;
        public uint TotalFiles;
        public string[] AllFiles;

        public Archive(string path, string format)
        {
            Filename = Path.GetFileNameWithoutExtension(path);
            Format = format;

            if (File.Exists(path))
            {
                if (Path.GetExtension(path) == "."+ Format)
                {
                    fs = new FileStream(path, FileMode.Open);
                    br = new BinaryReader(fs);

                    ReadHeader();
                    ReadItemEntries();
                    ExtractData();

                    br.Close();
                    fs.Close();

                    Log.Write("All files were successfully extracted to folder: " + Filename, Log.Code.Success);
                    
                }
                else
                {
                    Log.Write("File format is invalid, only ." + Format + " is accepted, enter to close!", Log.Code.Error);    
                }
            }
            else
            {
                AllFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).OrderBy(c => c.Length).ThenBy(c => c).ToArray();
                TotalFiles = (uint)AllFiles.Length;

                if (TotalFiles == 0)
                {

                    Log.Write("Empty folder, enter to close!", Log.Code.Error);
                }
                else
                {
                    string WriteToFile = Filename + "." + Format;

                    Util.MakeBackup(WriteToFile);

                    fs = new FileStream(Filename + "." + Format, FileMode.Create);
                    bw = new BinaryWriter(fs);

                    uint Offset = (uint)(4 + 12 * AllFiles.Length);

                    foreach (var FileEntry in AllFiles)
                    {
                        FileInfo Info = new FileInfo(FileEntry);

                        Item Entry = new Item();
                        Entry.Id = Convert.ToUInt32(Info.Name);
                        Entry.Offset = Offset;
                        Entry.Size = (uint)Info.Length;
                        Items.Add(Entry);

                        Offset = (uint)Util.NextPaddingOffset(Entry.Offset + Entry.Size, 4);
                    }

                    WriteHeader();
                    WriteItemEntries();
                    WriteData();

                    bw.Close();
                    fs.Close();

                    Log.Write("All files were successfully repacked to file: " + WriteToFile, Log.Code.Success);
                    
                }
            }
        }

        public void ReadHeader()
        {
            TotalFiles = br.ReadUInt32();
            Log.Write(TotalFiles + " files on archive.");
        }

        public void ReadItemEntries()
        {
            Log.Write("Getting files information.");

            for (int i = 0; i < TotalFiles; i++)
            {
                Item Entry = new Item();
                Entry.Id = br.ReadUInt32();
                Entry.Offset = br.ReadUInt32();
                Entry.Size = br.ReadUInt32();
                Items.Add(Entry);
            }
        }

        public void ExtractData()
        {
            Log.Write("Extracting files, this may take a while, be patient!", Log.Code.Warning);

            Directory.CreateDirectory(Filename);

            foreach (var Item in Items)
            {
                File.WriteAllBytes(Filename + "\\" + Item.Id, Util.GetFileData(br, Item.Offset, (int)Item.Size));
            }
        }

        public void WriteHeader()
        {
            Log.Write("Writing file header.");
            bw.Write(TotalFiles);
        }

        public void WriteItemEntries()
        {
            Log.Write("Writing file entries.");
            foreach (var Item in Items)
            {
                bw.Write(Item.Id);
                bw.Write(Item.Offset);
                bw.Write(Item.Size);
            }
        }

        public void WriteData()
        {
            Log.Write("Writing file data, this may take a while, be patient!", Log.Code.Warning);
            foreach (var Item in AllFiles)
            {
                bw.Write(File.ReadAllBytes(Item));
                bw.BaseStream.Position = Util.NextPaddingOffset((uint)bw.BaseStream.Position, 4);
            }
        }
    }
}

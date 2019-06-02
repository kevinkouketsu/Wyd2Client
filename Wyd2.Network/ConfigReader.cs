using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYD2.Common.GameStructure;

namespace WYD2.Control
{
    public static class ConfigReader
    {
        public static Dictionary<string, ushort> ReadItemEffect(string path)
        {
            Dictionary<string, ushort> retn = new Dictionary<string, ushort>();
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (!line.StartsWith("#define"))
                        continue;

                    char[] array = line.ToArray();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i] == '\t')
                            array[i] = ' ';
                    }

                    line = string.Empty;
                    foreach (var i in array)
                        line += i;

                    IList<string> texts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (texts == null)
                        continue;

                    retn[texts[1]] = ushort.Parse(texts[2]);
                }
            }

            return retn;
        }

        public static Dictionary<int, MItemList> ReadItemList(string path, string itemEffectPath)
        {
            Dictionary<int, MItemList> itemList = new Dictionary<int, MItemList>();
            Dictionary<string, ushort> itemEffect = ReadItemEffect(itemEffectPath);

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] items = line.Split(new char[] { ',' });
                    string[] meshBuf = items[2].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] scoreBuf = items[3].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    int itemIndex = short.Parse(items[0]);
                    if (itemIndex <= 0 || itemIndex >= Common.GameBasics.MAX_ITEMLIST)
                        continue;

                    MItemList item = new MItemList();
                    item.Effect = new List<MItemEffect>();
                    item.Name = items[1];
                    item.Unique = short.Parse(items[4]);
                    item.Price = int.Parse(items[5]);
                    item.Pos = short.Parse(items[6]);
                    item.Extreme = short.Parse(items[7]);
                    item.Grade = short.Parse(items[8]);

                    item.Mesh1 = short.Parse(meshBuf[0]);
                    item.Mesh2 = int.Parse(meshBuf[1]);

                    item.Level = short.Parse(scoreBuf[0]);
                    item.Str = short.Parse(scoreBuf[1]);
                    item.Int = short.Parse(scoreBuf[2]);
                    item.Dex = short.Parse(scoreBuf[3]);
                    item.Con = short.Parse(scoreBuf[4]);

                    int total = items.Length;
                    if (total >= 9)
                    {
                        for(int i = 9; i < total; i+= 2)
                        {
                            if (!itemEffect.ContainsKey(items[i]))
                                continue;

                            item.Effect.Add(new MItemEffect()
                            {
                                Index = itemEffect[items[i]],
                                Value = ushort.Parse(items[i + 1])
                            });
                        }
                    }

                    itemList[itemIndex] = item;
                }
            }

            return itemList;
        }
    }
}

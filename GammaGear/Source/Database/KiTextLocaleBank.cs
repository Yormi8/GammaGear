﻿using System.Collections.Generic;
using System.IO;

namespace GammaGear.Source.Database
{
    public class KiTextLocaleBank : KiLocaleBank, IKiLocaleBank
    {
        public KiTextLocaleBank(string localePath)
        {
            Init(localePath);
        }
        public override void Init(string localePath)
        {
            FileInfo file = new FileInfo(localePath);
            Path = file.FullName;
            using (var reader = new StreamReader(localePath))
            {
                Name = reader.ReadLine();
            }
            Loaded = false;
        }
        public override void LoadEntries()
        {
            if (Loaded)
            {
                return;
            }
            using (var reader = new StreamReader(Path))
            {
                Name = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string key = reader.ReadLine();
                    string comment = reader.ReadLine();
                    string content = reader.ReadLine();

                    _entries.Add(key, new Entry(comment, content));
                }
            }
            Loaded = true;
        }
    }
}
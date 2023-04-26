using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GammaGear.Source.Database
{
    public abstract class KiLocaleBank : IKiLocaleBank
    {
        public string Path { get; protected set; }
        public string Name { get; protected set; }
        public bool Loaded { get; protected set; }
        public abstract void Init(string path);
        public abstract void LoadEntries();
        public virtual string GetComment(string key)
        {
            return _entries.GetValueOrDefault(key).Comment;
        }
        public virtual string GetContent(string key)
        {
            return _entries.GetValueOrDefault(key).Content;
        }
        public virtual void LoadEntries(string path)
        {
            if (!Loaded)
            {
                Path = path;
                LoadEntries();
            }
        }
        protected struct Entry
        {
            public Entry(string comment, string content)
            {
                Comment = comment;
                Content = content;
            }
            public string Comment;
            public string Content;
        }
        protected Dictionary<string, Entry> _entries = new Dictionary<string, Entry>();
    }
}

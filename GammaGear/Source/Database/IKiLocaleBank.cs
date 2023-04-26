namespace GammaGear.Source.Database
{
    public interface IKiLocaleBank
    {
        public string Path { get; }
        public string Name { get; }
        public bool Loaded { get; }
        public void Init(string path);
        public string GetComment(string key);
        public string GetContent(string key);
        public void LoadEntries();
        public void LoadEntries(string Path);
    }
}

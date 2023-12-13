namespace GammaItems
{
    public abstract class ItemBase : object
    {
        public bool IsCustom;
        public Guid Id;

        public ItemBase(Guid Id, bool IsCustom)
        {
            this.Id = Id;
            this.IsCustom = IsCustom;
        }

        public ItemBase() : this(Guid.NewGuid(), true) { }

        public override bool Equals(object obj) => Equals(obj as ItemBase);

        public bool Equals(ItemBase other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

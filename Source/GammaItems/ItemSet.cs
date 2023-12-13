using System.Collections.ObjectModel;

namespace GammaItems
{
    public class ItemSet : ItemBase
    {
        public string SetName;
        public List<ItemSetBonus> Bonuses;

        public ItemSet(
            Guid Id,
            bool IsCustom,
            string SetName,
            IEnumerable<ItemSetBonus> Bonuses) :
                base(Id, IsCustom)
        {
            this.SetName = SetName;
            this.Bonuses = Bonuses.ToList();
        }

        public ItemSet() : base(Guid.NewGuid(), true) { }

        public override bool Equals(object obj) => Equals(obj as ItemSet);
        public bool Equals(ItemSet other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Id.Equals(other.Id);
        }
        public override int GetHashCode() => base.GetHashCode();
    }
}

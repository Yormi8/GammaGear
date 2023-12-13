using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaItems
{
    public class ItemSetBonus : Item
    {
        public int SetBonusLevel = -1;

        public ItemSetBonus() : base() { }
        public ItemSetBonus(Guid Id, bool IsCustom) : base(Id, IsCustom) { }
        public override bool Equals(object obj) => Equals(obj as ItemSetBonus);
        public bool Equals(ItemSetBonus obj)
        {
            return
                base.Equals(obj) &&
                SetBonusLevel == obj.SetBonusLevel;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}

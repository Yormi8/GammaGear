using System.Collections.Generic;
using GammaItems;

namespace GammaItems.Source.Database
{
    public interface IKiParser
    {
        PropertyClass ReadToPropertyClass(string path);
        IEnumerable<PropertyClass> ReadAllToPropertyClass(IEnumerable<string> paths);
        ItemBase ReadToItemBase(string path);
        ItemBase ReadToItemBase(PropertyClass propertyClass);
        IEnumerable<ItemBase> ReadAllToItemBase(IEnumerable<string> paths);
        IEnumerable<ItemBase> ReadAllToItemBase(IEnumerable<PropertyClass> propertyClasses);
    }
}

using System.Collections.Generic;

namespace GammaItems.Source.Database
{
    interface IKiParser
    {
        PropertyClass ReadToPropertyClass(string path);
        IEnumerable<PropertyClass> ReadAllToPropertyClass(IEnumerable<string> paths);
        KiObject ReadToKiObject(string path);
        KiObject ReadToKiObject(PropertyClass propertyClass);
        IEnumerable<KiObject> ReadAllToKiObject(IEnumerable<string> paths);
        IEnumerable<KiObject> ReadAllToKiObject(IEnumerable<PropertyClass> propertyClasses);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GammaItems.Source.Database
{
    public class KiJsonParser<T> : KiParser<T>
        where T : KiLocaleBank, new()
    {
        public KiJsonParser(string localePath) : base(localePath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(localePath);
            foreach (var file in directoryInfo.GetFiles())
            {
                T bank = new T();
                bank.Init(file.FullName);
                _banks.Add(bank.Name, bank);
            }

            _serializerSettings ??= CreateJsonSerializerSettings();
        }
        public override PropertyClass ReadToPropertyClass(string path)
        {
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);

            // Replace "__type" with "$type" and "class Whatever" with "Whatever"
            // Also fixes enums, with the "enum Foo::Bar::Value" being replaced with "Value"
            json = json.Replace("\"__type\"", "\"$type\"");
            json = Regex.Replace(json, "\"(?:class|enum) (?:[\\w]+\\:{2})*([\\w]*)\"", "\"$1\"");

            PropertyClass propertyClass = JsonConvert.DeserializeObject<PropertyClass>(json, _serializerSettings);

            return propertyClass;
        }

        protected static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Converters =
                {
                    new KiPropertyClassJsonConverter()
                }
            };
        }
        protected class KiPropertyClassJsonConverter : JsonConverter
        {
            private readonly Type[] acceptableTypes =
            {
                typeof(WizItemTemplate),
                typeof(ItemSetBonusTemplate),
                typeof(ItemSetBonusData),
                typeof(ProvideSpellEffectInfo),
                typeof(SpeedEffectInfo),
                typeof(TempStartingPipEffectInfo),
                typeof(StatisticEffectInfo),
                typeof(StartingPipEffectInfo),
                typeof(GameEffectInfo),
                typeof(RequirementList),
                typeof(ReqSchoolOfFocus),
                typeof(ReqHasBadge),
                typeof(ReqMagicLevel),
                typeof(Requirement),
                typeof(JewelSocketBehaviorTemplate),
                typeof(PetJewelBehaviorTemplate),
                typeof(PetItemBehaviorTemplate),
                typeof(BehaviorTemplate),
                typeof(JewelSocket),
                typeof(PropertyClass)
            };
            public override bool CanConvert(Type objectType)
            {
                return acceptableTypes.Any(t => t == objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // Maybe this works?
                if (reader.TokenType == JsonToken.Null)
                    return null;
                JObject jObject = JObject.Load(reader);
                string sourceType = jObject["$type"].Value<string>();

                object target = Activator.CreateInstance(acceptableTypes.FirstOrDefault(t => t.Name == sourceType) ?? objectType);

                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        protected JsonSerializerSettings _serializerSettings;
    }


}

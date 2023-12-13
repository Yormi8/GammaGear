using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;

namespace GammaItems.Source.Database
{
    /// <summary>
    /// It's super likely this class doesn't work, the new tool for extracting data from the
    /// game serializes to JSON instead of XML. This is kept in just incase we move back to XML.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("Use KiJsonParser<T> Instead", false)]
    public class KiXmlParser<T> : KiParser<T>
        where T: KiLocaleBank, new()
    {
        public KiXmlParser(string localePath) : base(localePath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(localePath);
            foreach (var file in directoryInfo.GetFiles())
            {
                T bank = new T();
                bank.Init(file.FullName);
                _banks.Add(bank.Name, bank);
            }

            _attributeOverrides ??= CreateXmlOverrides();
        }

        public override PropertyClass ReadToPropertyClass(string path)
        {
            if (!File.Exists(path)) return null;

            // Get xml content into a string.
            string xmlFileContent;
            using (var fileReader = new StreamReader(path))
            {
                xmlFileContent = fileReader.ReadToEnd();
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlFileContent);
            XmlNode rootNode = doc.DocumentElement;
            // Get all nodes with the name "Class"
            var nodes = rootNode.SelectNodes("//Class");

            // Replace <Class Name="class foo"> with just the name of the class.
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                XmlNode node = nodes[i];
                // create new (renamed) Content node
                XmlNode newNode = doc.CreateElement(node.Attributes[0].Value.Remove(0, 6));

                // [if needed] copy existing Content children
                newNode.InnerXml = node.InnerXml;

                // replace existing Content node with newly renamed Content node
                node.ParentNode.InsertBefore(newNode, node);
                node.ParentNode.RemoveChild(node);
            }

            // Read the new xml and deserialize into a root object.
            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);

            _serializer ??= new XmlSerializer(typeof(ItemObject), _attributeOverrides);
            ItemObject root = (ItemObject)_serializer.Deserialize(reader);

            if (root.m_propertyClasses.Count > 1)
            {
                Trace.WriteLine($"The XML file at {path} had more than one object defined.");
            }

            return root.m_propertyClasses.FirstOrDefault();
        }
        protected static XmlAttributeOverrides CreateXmlOverrides()
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();

            XmlAttributes MakeEnumAttr(string value)
            {
                XmlAttributes attrs = new XmlAttributes();
                XmlEnumAttribute enumAttr = new XmlEnumAttribute(value);
                attrs.XmlEnum = enumAttr;
                return attrs;
            }

            XmlAttributes MakeElementAttr(params string[] values)
            {
                XmlAttributes attrs = new XmlAttributes();
                foreach (var v in values)
                {
                    XmlElementAttribute elemAttr = new XmlElementAttribute(v);
                    attrs.XmlElements.Add(elemAttr);
                }
                return attrs;
            }

            XmlAttributes MakeElementAttrs(params (string value, Type type)[] values)
            {
                XmlAttributes attrs = new XmlAttributes();
                foreach (var v in values)
                {
                    var arrayItemAttr = new XmlElementAttribute(v.value, v.type);
                    attrs.XmlElements.Add(arrayItemAttr);
                }
                return attrs;
            }

            XmlAttributes MakeTypeAttr(string value)
            {
                XmlAttributes attrs = new XmlAttributes();
                XmlTypeAttribute typeAttr = new XmlTypeAttribute(value);
                attrs.XmlType = typeAttr;
                return attrs;
            }

            XmlAttributes MakeArrayItemAttr(params (string value, Type type)[] values)
            {
                XmlAttributes attrs = new XmlAttributes();
                foreach (var v in values)
                {
                    var arrayItemAttr = new XmlArrayItemAttribute(v.value, v.type);
                    attrs.XmlArrayItems.Add(arrayItemAttr);
                }
                return attrs;
            }

            XmlAttributes MakeRootAttr(string value)
            {
                XmlAttributes attrs = new XmlAttributes();
                var rootAttr = new XmlRootAttribute(value);
                attrs.XmlRoot = rootAttr;
                return attrs;
            }

            // enum RarityType
            overrides.Add(typeof(RarityType), "RT_COMMON",      MakeEnumAttr("enum RarityType::RT_COMMON"));
            overrides.Add(typeof(RarityType), "RT_UNCOMMON",    MakeEnumAttr("enum RarityType::RT_UNCOMMON"));
            overrides.Add(typeof(RarityType), "RT_RARE",        MakeEnumAttr("enum RarityType::RT_RARE"));
            overrides.Add(typeof(RarityType), "RT_ULTRARARE",   MakeEnumAttr("enum RarityType::RT_ULTRARARE"));
            overrides.Add(typeof(RarityType), "RT_EPIC",        MakeEnumAttr("enum RarityType::RT_EPIC"));

            // enum OperatorType
            overrides.Add(typeof(OperatorType), "ROP_AND",  MakeEnumAttr("enum Requirement::Operator::ROP_AND"));
            overrides.Add(typeof(OperatorType), "ROP_OR",   MakeEnumAttr("enum Requirement::Operator::ROP_OR"));

            // enum NumericOperatorType
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_UNKNOWN",          MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_UNKNOWN"));
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_EQUALS",           MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_EQUALS"));
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_GREATER_THAN",     MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_GREATER_THAN"));
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_LESS_THAN",        MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_LESS_THAN"));
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_GREATER_THAN_EQ",  MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_GREATER_THAN_EQ"));
            overrides.Add(typeof(NumericOperatorType), "OPERATOR_LESS_THAN_EQ",     MakeEnumAttr("enum ReqNumeric::OPERATOR_TYPE::OPERATOR_LESS_THAN_EQ"));

            // enum JewelSocketType
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_SQUARE",             MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_SQUARE"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_CIRCLE",             MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_CIRCLE"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_TRIANGLE",           MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_TRIANGLE"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_TEAR",               MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_TEAR"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_PET",                MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_PET"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_PINSQUAREPIP",       MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUAREPIP"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_PINSQUARESHIELD",    MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUARESHIELD"));
            overrides.Add(typeof(JewelSocketType), "SOCKETTYPE_PINSQUARESWORD",     MakeEnumAttr("enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUARESWORD"));

            // enum BooleanEnum is already setup for default

            // class PropertyClass
            overrides.Add(typeof(PropertyClass), MakeTypeAttr("PropertyClass"));

            // class JewelSocket
            overrides.Add(typeof(JewelSocket), MakeTypeAttr("JewelSocket"));
            overrides.Add(typeof(JewelSocket), "m_socketType", MakeElementAttr("m_socketType"));
            overrides.Add(typeof(JewelSocket), "m_bLockable",  MakeElementAttr("m_bLockable"));

            // class BehaviorTemplate
            overrides.Add(typeof(BehaviorTemplate), MakeTypeAttr("BehaviorTemplate"));
            overrides.Add(typeof(BehaviorTemplate), "m_behaviorName", MakeElementAttr("m_behaviorName"));

            // class PetItemBehaviorTemplate
            overrides.Add(typeof(PetJewelBehaviorTemplate), MakeTypeAttr("PetJewelBehaviorTemplate"));
            overrides.Add(typeof(PetJewelBehaviorTemplate), "m_petTalentName", MakeElementAttr("m_petTalentName"));
            overrides.Add(typeof(PetJewelBehaviorTemplate), "m_minPetLevel", MakeElementAttr("m_minPetLevel"));

            // class JewelSocketBehaviorTemplate
            overrides.Add(typeof(JewelSocketBehaviorTemplate), MakeTypeAttr("JewelSocketBehaviorTemplate"));
            overrides.Add(typeof(JewelSocketBehaviorTemplate), "m_jewelSockets", MakeArrayItemAttr(("m_petTalentName", typeof(JewelSocket))));
            overrides.Add(typeof(JewelSocketBehaviorTemplate), "m_socketDeleted", MakeElementAttr("m_socketDeleted"));

            // class Requirement
            overrides.Add(typeof(Requirement), MakeTypeAttr("Requirement"));
            overrides.Add(typeof(Requirement), "m_applyNOT", MakeElementAttr("m_applyNOT"));
            overrides.Add(typeof(Requirement), "m_operator", MakeElementAttr("m_operator"));

            // class ReqMagicLevel
            overrides.Add(typeof(ReqMagicLevel), MakeTypeAttr("ReqMagicLevel"));
            overrides.Add(typeof(ReqMagicLevel), "m_numericValue", MakeElementAttr("m_numericValue"));
            overrides.Add(typeof(ReqMagicLevel), "m_operatorType", MakeElementAttr("m_operatorType"));
            overrides.Add(typeof(ReqMagicLevel), "m_magicSchool", MakeElementAttr("m_magicSchool"));

            // class ReqHasBadge
            overrides.Add(typeof(ReqHasBadge), MakeTypeAttr("ReqHasBadge"));
            overrides.Add(typeof(ReqHasBadge), "m_badgeName", MakeElementAttr("m_badgeName"));

            // class ReqSchoolOfFocus
            overrides.Add(typeof(ReqSchoolOfFocus), MakeTypeAttr("ReqSchoolOfFocus"));
            overrides.Add(typeof(ReqSchoolOfFocus), "m_magicSchool", MakeElementAttr("m_magicSchool"));

            // class RequirementList
            overrides.Add(typeof(RequirementList), MakeTypeAttr("RequirementList"));
            overrides.Add(typeof(RequirementList), "m_applyNOT", MakeElementAttr("m_applyNOT"));
            overrides.Add(typeof(RequirementList), "m_operator", MakeElementAttr("m_operator"));
            overrides.Add(typeof(RequirementList), "m_requirements", MakeArrayItemAttr(
                ("ReqSchoolOfFocus", typeof(ReqSchoolOfFocus)),
                ("ReqMagicLevel", typeof(ReqMagicLevel)),
                ("ReqHasBadge", typeof(ReqHasBadge))
            ));

            // class GameEffectInfo
            overrides.Add(typeof(GameEffectInfo), MakeTypeAttr("GameEffectInfo"));
            overrides.Add(typeof(GameEffectInfo), "m_effectName", MakeElementAttr("m_effectName"));

            // class StartingPipEffectInfo
            overrides.Add(typeof(StartingPipEffectInfo), MakeTypeAttr("StartingPipEffectInfo"));
            overrides.Add(typeof(StartingPipEffectInfo), "m_pipsGiven", MakeElementAttr("m_pipsGiven"));
            overrides.Add(typeof(StartingPipEffectInfo), "m_powerPipsGiven", MakeElementAttr("m_powerPipsGiven"));

            // class TempStartingPipEffectInfo
            overrides.Add(typeof(TempStartingPipEffectInfo), MakeTypeAttr("TempStartingPipEffectInfo"));

            // class StatisticEffectInfo
            overrides.Add(typeof(StatisticEffectInfo), MakeTypeAttr("StatisticEffectInfo"));
            overrides.Add(typeof(StatisticEffectInfo), "m_lookupIndex", MakeElementAttr("m_lookupIndex"));

            // class SpeedEffectInfo
            overrides.Add(typeof(SpeedEffectInfo), MakeTypeAttr("SpeedEffectInfo"));
            overrides.Add(typeof(SpeedEffectInfo), "m_speedMultiplier", MakeElementAttr("m_speedMultiplier"));

            // class ProvideSpellEffectInfo
            overrides.Add(typeof(ProvideSpellEffectInfo), MakeTypeAttr("ProvideSpellEffectInfo"));
            overrides.Add(typeof(ProvideSpellEffectInfo), "m_spellName", MakeElementAttr("m_spellName"));
            overrides.Add(typeof(ProvideSpellEffectInfo), "m_numSpells", MakeElementAttr("m_numSpells"));

            // class ItemSetBonusData
            overrides.Add(typeof(ItemSetBonusData), MakeTypeAttr("ItemSetBonusData"));
            overrides.Add(typeof(ItemSetBonusData), "m_numItemsToEquip", MakeElementAttr("m_numItemsToEquip"));
            overrides.Add(typeof(ItemSetBonusData), "m_description", MakeElementAttr("m_description"));
            overrides.Add(typeof(ItemSetBonusData), "m_equipEffectsGrantedRequirements", MakeArrayItemAttr(
                ("RequirementList", typeof(RequirementList))
            ));
            overrides.Add(typeof(ItemSetBonusData), "m_equipEffectsGranted", MakeArrayItemAttr(
                ("StatisticEffectInfo", typeof(StatisticEffectInfo)),
                ("ProvideSpellEffectInfo", typeof(ProvideSpellEffectInfo)),
                ("StartingPipEffectInfo", typeof(StartingPipEffectInfo)),
                ("TempStartingPipEffectInfo", typeof(TempStartingPipEffectInfo))
            ));

            // class ItemSetBonusTemplate
            overrides.Add(typeof(ItemSetBonusTemplate), MakeTypeAttr("ItemSetBonusTemplate"));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_behaviors", MakeArrayItemAttr(
                ("PetItemBehaviorTemplate", typeof(PetItemBehaviorTemplate)),
                ("PetJewelBehaviorTemplate", typeof(PetJewelBehaviorTemplate)),
                ("JewelSocketBehaviorTemplate", typeof(JewelSocketBehaviorTemplate))
            ));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_objectName", MakeElementAttr("m_objectName"));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_templateID", MakeElementAttr("m_templateID"));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_displayName", MakeElementAttr("m_displayName"));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_noStacking", MakeElementAttr("m_noStacking"));
            overrides.Add(typeof(ItemSetBonusTemplate), "m_itemSetBonusDataList", MakeArrayItemAttr(
                ("ItemSetBonusData", typeof(ItemSetBonusData))
            ));

            // class WizItemTemplate
            overrides.Add(typeof(WizItemTemplate), MakeTypeAttr("WizItemTemplate"));
            overrides.Add(typeof(WizItemTemplate), "m_behaviors", MakeArrayItemAttr(
                ("PetItemBehaviorTemplate", typeof(PetItemBehaviorTemplate)),
                ("PetJewelBehaviorTemplate", typeof(PetJewelBehaviorTemplate)),
                ("JewelSocketBehaviorTemplate", typeof(JewelSocketBehaviorTemplate))
            ));
            overrides.Add(typeof(WizItemTemplate), "m_objectName", MakeElementAttr("m_objectName"));
            overrides.Add(typeof(WizItemTemplate), "m_templateID", MakeElementAttr("m_templateID"));
            overrides.Add(typeof(WizItemTemplate), "m_adjectiveList", MakeElementAttr("m_adjectiveList"));
            overrides.Add(typeof(WizItemTemplate), "m_displayName", MakeElementAttr("m_displayName"));
            overrides.Add(typeof(WizItemTemplate), "m_description", MakeElementAttr("m_description"));
            overrides.Add(typeof(WizItemTemplate), "m_sIcon", MakeElementAttr("m_sIcon"));
            overrides.Add(typeof(WizItemTemplate), "m_equipRequirements", MakeArrayItemAttr(
                ("RequirementList", typeof(RequirementList))
            ));
            overrides.Add(typeof(WizItemTemplate), "m_equipEffects", MakeArrayItemAttr(
                ("StatisticEffectInfo", typeof(StatisticEffectInfo)),
                ("ProvideSpellEffectInfo", typeof(ProvideSpellEffectInfo)),
                ("StartingPipEffectInfo", typeof(StartingPipEffectInfo)),
                ("TempStartingPipEffectInfo", typeof(TempStartingPipEffectInfo))
            ));
            overrides.Add(typeof(WizItemTemplate), "m_itemSetBonusTemplateID", MakeElementAttr("m_itemSetBonusTemplateID"));
            overrides.Add(typeof(WizItemTemplate), "m_school", MakeElementAttr("m_school"));
            overrides.Add(typeof(WizItemTemplate), "m_rarity", MakeElementAttr("m_rarity"));

            // class ItemObject
            overrides.Add(typeof(ItemObject), "m_itemSetBonusTemplateID", MakeRootAttr("ItemObject"));
            overrides.Add(typeof(ItemObject), "m_propertyClasses", MakeElementAttrs(
                ("WizItemTemplate", typeof(WizItemTemplate)),
                ("ItemSetBonusTemplate", typeof(ItemSetBonusTemplate))
            ));

            return overrides;
        }

        protected XmlSerializer _serializer;
        protected XmlAttributeOverrides _attributeOverrides;
    }
}

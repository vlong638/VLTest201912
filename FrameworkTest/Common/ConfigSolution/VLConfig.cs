﻿using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FrameworkTest.Common.ConfigSolution
{

    public class VLConfig
    {
        public List<XMLConfigItem> Items { set; get; }
        public List<XMLConfigItemSet> ItemSets { set; get; }
        public Dictionary<string, string> Dic { get; private set; }

        public VLConfig()
        {
        }

        public VLConfig(string text)
        {
            if (text.IsNullOrEmpty())
                throw new NotImplementedException("配置不能为空");
            XDocument doc = XDocument.Parse(text);
            Items = doc.Descendants(XMLConfigItem.ElemntName).Select(c => new XMLConfigItem(c)).ToList() ?? new List<XMLConfigItem>();
            ItemSets = doc.Descendants(XMLConfigItemSet.ElemntName).Select(c => new XMLConfigItemSet(c)).ToList() ?? new List<XMLConfigItemSet>();
            Dic = this.GetKeyValues();
        }

        internal Dictionary<string, string> GetKeyValues()
        {
            var result = new Dictionary<string, string>();
            foreach (var keyValue in Items.Select(c => c.GetKeyValue()))
            {
                result.Add(keyValue.Key, keyValue.Value);
            }
            foreach (var keyValueSets in ItemSets.Select(c => c.GetKeyValues()))
            {
                foreach (var keyValue in keyValueSets)
                {
                    result.Add(keyValue.Key, keyValue.Value);
                }
            }
            return result;
        }

        public string GetKey(string key)
        {
            if (Dic.ContainsKey(key))
            {
                return Dic[key];
            }
            return null;
        }
    }

    public class XMLConfigItem
    {
        public const string ElemntName = "item";

        public XMLConfigItem()
        {
        }

        public XMLConfigItem(XElement element)
        {


            Element = element;
            Key = element.Attribute(nameof(Key).ToLower()).Value;
            Value = element.Attribute(nameof(Value).ToLower()).Value;
        }

        public XElement Element { get; }
        public string Key { set; get; }
        public string Value { set; get; }

        internal KeyValuePair<string, string> GetKeyValue()
        {
            return new KeyValuePair<string, string>(Key, Value);
        }
    }

    public class XMLConfigItemSet
    {
        public const string ElemntName = "itemset";
        public const string Linker = "_";

        public XMLConfigItemSet()
        {
        }

        public XMLConfigItemSet(XElement element)
        {
            Element = element;
            Key = element.Attribute(nameof(Key).ToLower()).Value;
            Items = element.Descendants(XMLConfigItem.ElemntName).Select(c => new XMLConfigItem(c)).ToList() ?? new List<XMLConfigItem>();
            ItemSets = element.Descendants(XMLConfigItemSet.ElemntName).Select(c => new XMLConfigItemSet(c)).ToList() ?? new List<XMLConfigItemSet>();
        }

        public XElement Element { get; }
        public string Key { set; get; }
        public List<XMLConfigItem> Items { set; get; }
        public List<XMLConfigItemSet> ItemSets { set; get; }

        internal List<KeyValuePair<string, string>> GetKeyValues()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            result.AddRange(Items.Select(c => GetLinkedKeyValue(c.GetKeyValue())));
            foreach (var keyValues in ItemSets.Select(c => c.GetKeyValues()))
            {
                result.AddRange(keyValues.Select(c => GetLinkedKeyValue(c)));
            }
            return result;
        }

        private KeyValuePair<string, string> GetLinkedKeyValue(KeyValuePair<string, string> keyValuePair)
        {
            return new KeyValuePair<string, string>(Key + Linker + keyValuePair.Key, keyValuePair.Value);
        }
    }
}
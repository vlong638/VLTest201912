using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.Common.XMLSolution
{
    public static class XMLHelper
    {
        /// <summary>
        /// 创建简单的xml并保存
        /// </summary>
        public static void TestCreate(string path)
        {
            var root = new XElement("root");
            var tableA = new XElement("Table"
                , new XElement("Field", 1)
                , new XElement("Field", 2)
                , new XElement("Field", 3)
                );
            tableA.SetAttributeValue("TableName", "tableA");
            var tableB = new XElement("Table"
                , new XElement("Field", 1)
                , new XElement("Field", 2)
                , new XElement("Field", 3)
                );
            tableB.SetAttributeValue("TableName", "tableB");
            var items = new List<XElement>()
            {
                tableA,
                tableB,
            };
            root.Add(items);
            XDocument xdoc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"), root);
            xdoc.Save(path);
        }

        public static XElement ToXElement<T>(this T entity) where T : class
        {
            var type = entity.GetType();
            var properties = type.GetProperties();
            var element = new XElement(type.Name);
            foreach (var property in properties)
            {
                element.SetAttributeValue(property.Name, property.GetValue(entity));
            }
            return element;
        }

        public static void SaveAs(this XElement element,string path)
        {
            XDocument xdoc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"), element);
            xdoc.Save(path);
        }
    }
}

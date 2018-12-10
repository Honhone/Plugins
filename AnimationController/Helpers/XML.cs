using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.Networking.Match;

namespace AnimationController
{
    public class XML<T>
    {
        private static XmlSerializer Serializer => new XmlSerializer(typeof(T));
        private const string pattern = " xmlns:xs[id]=\"[^\"]+\"";

        public static T From(string xml)
        {
            using (var stream = new StringReader(xml))
                return (T)Serializer.Deserialize(stream);
        }

        //public static string To(T obj)
        //{
        //    var sb = new StringBuilder();
        //    using (var stream = new StringWriter(sb))
        //        Serializer.Serialize(stream, obj);

        //    return sb.ToString();
        //}
        public static string SerializeObject(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;  
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }
                return Regex.Replace(textWriter.ToString(), pattern, string.Empty); 
            }
        }
    }
}


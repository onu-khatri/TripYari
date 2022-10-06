using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;
using System.Xml.Serialization;

namespace TripYari.Core.Base.Providers.Xml
{
    public class XmlSerializerOutputFormatterNamespace : XmlSerializerOutputFormatter
    {
        public XmlSerializerOutputFormatterNamespace(XmlWriterSettings xmlWriterSettings):base(xmlWriterSettings)
        {
            
        }
        protected override void Serialize(XmlSerializer xmlSerializer, XmlWriter xmlWriter, object value)
        {
            //applying "empty" namespace will produce no namespaces
            var emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "any-non-empty-string");
            xmlSerializer.Serialize(xmlWriter, value, emptyNamespaces);
        }
    }
}

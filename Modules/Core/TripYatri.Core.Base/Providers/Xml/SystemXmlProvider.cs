using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TripYatri.Core.Base.Providers.Xml
{
    public class SystemXmlProvider : IXmlProvider
    {
        #region Serialize

        /// <summary>
        /// Serializes the specified object. Unicode is used
        /// </summary>
        /// <param name="obj">the object to serilize</param>
        /// <typeparam name="T">Any Type of Object</typeparam>
        /// <returns>xml string copy of the object</returns>
        public string Serialize<T>(T obj)
        {
            return Serialize(obj, Encoding.Unicode);
        }

        /// <summary>
        /// Serializes the specified tag to xml string.
        /// </summary>
        /// <param name="tag">The object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <typeparam name="T">Any Type of Object</typeparam>
        /// <returns>xml string copy of the object</returns>
        private static string Serialize<T>(T tag, Encoding encoding)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, encoding);
            var serializer = new XmlSerializer(tag.GetType());
            serializer.Serialize(streamWriter, tag);

            using var streamReader = new StreamReader(memoryStream, encoding);
            memoryStream.Position = 0;
            return streamReader.ReadToEnd();
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes the specified XML data to Generic Type.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <typeparam name="T">Any Type of Object</typeparam>
        /// <returns>The generic object</returns>
        public T Deserialize<T>(string xmlData)
        {
            using var stringReader = new StringReader(xmlData);
            var serializer = new XmlSerializer(typeof(T));
            var tag = (T) serializer.Deserialize(stringReader);
            return tag;
        }

        #endregion
    }
}
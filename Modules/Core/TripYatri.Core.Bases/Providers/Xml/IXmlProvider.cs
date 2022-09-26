namespace TripYatri.Core.Base.Providers.Xml
{
    public interface IXmlProvider
    {
        /// <summary>
        /// Deserializes the specified XML data to Generic Type.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <returns>The generic object</returns>
        T Deserialize<T>(string xmlData);

        /// <summary>
        /// Seerializes the specified Generic Type to XML data.
        /// </summary>
        /// <typeparam name="T">Any Type of Object</typeparam>
        /// <returns>The generic object</returns>
        string Serialize<T>(T typeObj);
    }
}
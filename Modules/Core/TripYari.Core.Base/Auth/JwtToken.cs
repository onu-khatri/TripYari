// ReSharper disable InconsistentNaming

namespace TripYari.Core.Base.Auth
{
    [Serializable]
    public class JwtToken
    {
        public string account_id { get; set; }
        public string iss { get; set; }
        public string sub { get; set; }
        public string aud { get; set; }
        public int iat;
        /// <summary>
        /// Epoch date of the expiration of this JWT token
        /// </summary>
        public int exp { get; set; }
    }
}

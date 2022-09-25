namespace TripYatri.Core.API.Auth
{
    public class BearerToken
    {
        public BearerToken(string clientId, string accountDid, string userDid)
        {
            ClientId = clientId;
            AccountDid = accountDid;
            UserDid = userDid;
        }

        public BearerToken(string clientId, string accountDid, string userDid, string ownerId)
        {
            ClientId = clientId;
            AccountDid = accountDid;
            UserDid = userDid;
            OwnerId = ownerId;
        }

        public string ClientId { get; }
        public string OwnerId { get; }
        public string UserDid { get; }
        public string AccountDid { get; }
    }
}

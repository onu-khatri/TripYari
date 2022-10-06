// ReSharper disable InconsistentNaming

namespace TripYari.Core.RuntimeContext
{
    public class RuntimeEnvironment
    {
        public static RuntimeEnvironment produs { get; } = new RuntimeEnvironment("produs", "api.careerbuilder.com");
        public static RuntimeEnvironment prodeu { get; } = new RuntimeEnvironment("prodeu", "api.careerbuilder.eu");

        public static RuntimeEnvironment stage { get; } =
            new RuntimeEnvironment("stage", "wwwtest.api.careerbuilder.com");

        public static RuntimeEnvironment development { get; } =
            new RuntimeEnvironment("development", "wwwtest.api.careerbuilder.com");

        public static RuntimeEnvironment FromName(string environmentName)
        {
            switch (environmentName.ToLower())
            {
                case "produs":
                case "production":
                case "productionus":
                case "usproduction":
                    return produs;
                case "prodeu":
                case "productioneu":
                case "euproduction":
                    return prodeu;
                case "stage":
                case "staging":
                case "test":
                    return stage;
                case "dev":
                case "development":
                    return development;
            }

            Console.Error.WriteLine(
                $"Could not determine the Runtime Environment from the dotnet Environment {environmentName}. Did you forget to override the environment variable DOTNET_ENVIRONMENT?");
            return development;
        }

        public string Name { get; }
        public string OAuthServer { get; }
        public string OAuthRuntimeContextUri => $"https://{OAuthServer}";

        private RuntimeEnvironment(string name, string oauthServer)
        {
            Name = name;
            OAuthServer = oauthServer;
        }

        public bool IsDevelopment()
        {
            // return Name == "development";
            return this == development;
        }

        public bool IsStaging()
        {
            return this == stage;
        }

        public bool IsProduction()
        {
            return this == produs || this == prodeu;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
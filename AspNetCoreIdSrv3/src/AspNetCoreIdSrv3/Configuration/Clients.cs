using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace AspNetCoreIdSrv3.Configuration
{
    public class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Test Client",
                    ClientId = "test",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    // server to server communication
                    Flow = Flows.ResourceOwner,

                    // only allowed to access api1
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "email",
                        "profile",
                        "api1"
                    }
                }
            };
        }
    }
}
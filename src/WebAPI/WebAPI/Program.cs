using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
         WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((ctx, builder) =>
             {
                 var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                 var isDevelopment = environment == EnvironmentName.Development;

                 if (!isDevelopment)
                 {
                     var config = builder.Build();
                     string keyvaultName = $"{config["KeyVaultName"]}-{config["Environment"]}";
                     var keyVaultEndpoint = $"https://{keyvaultName}.vault.azure.net/";
                     if (!string.IsNullOrEmpty(keyVaultEndpoint))
                     {
                         var azureServiceTokenProvider = new AzureServiceTokenProvider();
                         var keyVaultClient = new KeyVaultClient(
                             new KeyVaultClient.AuthenticationCallback(
                                 azureServiceTokenProvider.KeyVaultTokenCallback));
                         builder.AddAzureKeyVault(
                             keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                     }
                 }
             }
          ).UseStartup<Startup>()
.Build();
    }
}

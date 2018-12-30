using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace DataFunctions
{
    public class StartUp
    {

    }

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var builder = new ConfigurationBuilder();
            var config = builder
                                .SetBasePath(context.FunctionAppDirectory)
                                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                                .AddEnvironmentVariables()
                                .AddUserSecrets<StartUp>()
                                .Build();

            string keyvaultName = $"{config["KeyVaultName"]}-{config["Environment"]}";
            var keyVaultEndpoint = $"https://{keyvaultName}.vault.azure.net/";

            //I cant find any info on how to spin up a IHostingEnvironment for Azure function, so I do this manually. Not optimal.
            string coreDevEnv = config["ASPNETCORE_ENVIRONMENT"];
            bool isDev = config["ASPNETCORE_ENVIRONMENT"] == "Development";
            if (!isDev)
            {
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

            config = builder.Build();
            string secret = config["ARMSecret"];

            return secret != null
                ? (ActionResult)new OkObjectResult($"Hello (updated 4), {secret}")
                : new BadRequestObjectResult("Error");
        }
    }
}

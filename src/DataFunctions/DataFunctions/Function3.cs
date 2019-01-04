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
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace DataFunctions
{
    public static class Function3
    {
        private static IConfigurationBuilder _builder = new ConfigurationBuilder()
                        .SetBasePath(GetWorkingDir())
                        .AddJsonFile("settings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<StartUp>();
        private static IConfigurationRoot _config;

        private static string GetWorkingDir()
        {
            string current = Environment.CurrentDirectory;
            string dir = current;

            if (current.Contains("system32")) //this is a hack to get things working on Azure. I need to figure out a better way of doing this.
                dir = @"D:\home\site\wwwroot";

            return dir;
        }


        static Function3()
        {
            _config = _builder.Build();
            string keyvaultName = $"{_config["KeyVaultName"]}-{_config["Environment"]}";
            var keyVaultEndpoint = $"https://{keyvaultName}.vault.azure.net/";

            //I cant find any info on how to spin up a IHostingEnvironment for Azure function, so I do this manually. Not optimal.
            string coreDevEnv = _config["ASPNETCORE_ENVIRONMENT"];
            bool isDev = _config["ASPNETCORE_ENVIRONMENT"] == "Development";
            if (!isDev)
            {
                if (!string.IsNullOrEmpty(keyVaultEndpoint))
                {
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));
                    _builder.AddAzureKeyVault(
                        keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                    _config = _builder.Build();
                }
            }

        }

        [FunctionName("Function3")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, 
            [Queue("testqueue", Connection ="StorageQueueConnectionString")]
            ICollector<string> outputQueueItem,
            ILogger log)
        {
            string secret = _config["ARMSecret"];

            outputQueueItem.Add(secret);

            return ? (ActionResult)new OkObjectResult($"ARMSecret, {secret}")
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Models;
using System.Net;
using Microsoft.Azure.Devices;

namespace AzureFunctions
{
    public class RegisterDevice
    {
        private RegistryManager _registryManager;

        public RegisterDevice(RegistryManager registryManager)
        {
            _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
        }

        [FunctionName("RegisterDevice")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            RegisterDeviceRequestModel data = JsonConvert.DeserializeObject<RegisterDeviceRequestModel>(await new StreamReader(req.Body).ReadToEndAsync());

            if (!string.IsNullOrEmpty(data?.DeviceId))
            {
                var device = await _registryManager.AddDeviceAsync(new Device(data!.DeviceId));

                var response = req.CreateResponse(HttpStatusCode.Created);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                return new OkObjectResult(new RegisterDeviceResponseModel
                {

                });
            }

            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    internal class RegisterDeviceRequestModel
    {
        public string DeviceId { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
    }
}

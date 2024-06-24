using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetworkInformation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetworkInformationController: ControllerBase {

        [HttpGet("get-client-hostname")]
        public IActionResult GetClientHostName() {
            var ipAddress = GetClientIpAddress(HttpContext);
            var hostName = GetClientHostName(ipAddress);

            return Ok(new { IpAddress = ipAddress, HostName = hostName });
        }

        public string GetClientIpAddress(HttpContext context) {
            // Obtener la IP desde los encabezados si está detrás de un proxy.
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                var forwardedAddresses = forwardedHeader.Split(',');
                if (forwardedAddresses.Length > 0)
                {
                    return forwardedAddresses[0].Trim();
                }
            }

            var realIpHeader = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIpHeader))
            {
                return realIpHeader;
            }

            // Usar la IP remota si no hay encabezados de proxy.
            var ipAddress = context.Connection.RemoteIpAddress;
            return ipAddress?.ToString();
        }

        private string GetClientHostName(string ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry.HostName;
            }
            catch
            {
                return "Unknown";
            }
        }

        public NetworkInformationController() {

        }
    }
}

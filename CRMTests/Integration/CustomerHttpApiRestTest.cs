using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using Xunit;

namespace CRMTests.Integration
{
    public class CustomerHttpApiRestTest
    {
        private static readonly HttpClient client = new HttpClient();

        [Fact]
        public async void CustomerPostEndpointPingShouldRespondAlive()
        {
            using var process = new Process();
            string exePath = "/home/runner/work/SimpleCRM/SimpleCRM/CRMTests/bin/Debug/net6.0/CRMRestApiV2.exe";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exePath = "C:\\Users\\SteevesSaillant\\source\\repos\\XUnit_CSharp_Example\\CRMRestApiV2\\bin\\Debug\\net6.0\\CRMRestApiV2.exe";
            }

            process.StartInfo.FileName = exePath;
            process.Start();


            client.BaseAddress = new Uri("http://localhost:5000");
            var response = await client.GetAsync("/api/CRMCustomer/");
            Assert.True(response.StatusCode.Equals(HttpStatusCode.OK));
            process.Kill();
        }
    }
}

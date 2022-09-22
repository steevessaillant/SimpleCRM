using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using Xunit;

namespace CRMTests.Integration
{
    [ExcludeFromCodeCoverage]
    public class CustomerHttpApiRestTest
    {
        private static readonly HttpClient client = new HttpClient();

        //maybe use this https://github.com/stephenhillier/apitest#complete-example
        //Work In progress

        [Fact]
        public async void CustomerPostEndpointPingShouldRespondAlive()
        {

            using var process = new Process();
            string exePath = "/home/runner/work/SimpleCRM/SimpleCRM/CRMRestApiV2/bin/Debug/net6.0/CRMRestApiV2.exe";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exePath = "C:\\Users\\SteevesSaillant\\source\\repos\\XUnit_CSharp_Example\\CRMRestApiV2\\bin\\Debug\\net6.0\\CRMRestApiV2.exe";
            }
            else
            {
                
                //pass on github action for now.
                string command = "cd ../../../;chmod -R 777 /home/runner/work/SimpleCRM/SimpleCRM/CRMRestApiV2/bin/Debug/net6.0";
                string result = "";
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "/bin/bash";
                    proc.StartInfo.Arguments = "-c \" " + command + " \"";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();

                    result += proc.StandardOutput.ReadToEnd();
                    result += proc.StandardError.ReadToEnd();

                    proc.WaitForExit();
                }
                Console.Write(result);
            }

            process.StartInfo.FileName = exePath;
            process.Start();


            client.BaseAddress = new Uri("http://localhost:5222");
            var response = await client.GetAsync("/api/CRMCustomer/");
            Assert.True(response.StatusCode.Equals(HttpStatusCode.OK));
            process.Kill();
        }
    }
}

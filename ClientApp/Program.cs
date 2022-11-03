// using System.Net.Http;
// using System.Security.Authentication;
// using System.Security.Cryptography.X509Certificates;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Authentication;
using ClientApp;

AuthJwt ob1 = new AuthJwt();
var token = ob1.generatejwt(10);

X509Certificate2 clientCert = certval.GetClientCertificate();


var handler = new HttpClientHandler();
handler.ClientCertificateOptions = ClientCertificateOption.Manual;
handler.SslProtocols = SslProtocols.Tls12;
//var clientCert = new X509Certificate2("C:\\Certificates\\client.pfx","Pa$$w0rd");
//var cert= X509Certificate certificate = GetCert("ClientCertificate.cer");
//var thb = cert.Thumbprint;
Console.WriteLine(clientCert.Thumbprint);
handler.ClientCertificates.Add(clientCert);
//Console.WriteLine(thb);
//var client = new HttpClient();
var client = new HttpClient(handler);
//client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

var result = client.GetStringAsync("https://restapi20220930115015.azurewebsites.net/WeatherForecast").GetAwaiter().GetResult();
Console.WriteLine(result);
Console.ReadKey();






//namespace ClientApp
//{
//    internal class Program
//    {
//        static async Task Main(string[] args)
//        {
//            await SendRequest();
//            Console.ReadKey();
//        }

//        static async Task SendRequest()
//        {

//            try
//            {
//                var cert = new X509Certificate2("C:\\Certificates\\client.pfx", "Pa$$w0rd");
//                var handler = new HttpClientHandler();
//                handler.ClientCertificates.Add(cert);
//                var client = new HttpClient(handler);

//                var request = new HttpRequestMessage()
//                {
//                    RequestUri = new Uri("https://localhost:443/home/privacy"),
//                    Method = HttpMethod.Get,
//                };

//                var response = await client.SendAsync(request);

//                Console.WriteLine(response.StatusCode);
//                if (response.IsSuccessStatusCode)
//                {
//                    var responseContent = await response.Content.ReadAsStringAsync();
//                    Console.WriteLine(responseContent);
//                }
//            }

//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//            }
//        }
//    }
//}

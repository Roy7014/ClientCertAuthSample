using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
internal class CertificateValidation
{
    private readonly ILogger _logger;
    //public CertificateValidation(ILogger<CertificateValidation> logger)
    //{
    //    _logger = logger;
    //}
    public bool ValidateCertificate(X509Certificate2 clientCertificate)
    {


        // var cert = new X509Certificate2("C:\\home\\site\\wwwroot\\Certificates\\JPMC.cer");
        //var thb = cert.Thumbprint;
        var allowedThumbprints1 = "81c66b4752a48d29cb29b0ce2708092615786073";
        //_logger.LogInformation("Input:" + clientCertificate.Thumbprint);
        if (string.Equals(allowedThumbprints1, clientCertificate.Thumbprint, System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;

    }
}
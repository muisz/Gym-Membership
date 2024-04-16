using System.Text;

namespace GymMembership.Services
{
    public class MidtransPaymentClient
    {
        private readonly IConfiguration _configuration;
        private readonly bool isProduction;
        private readonly string clientKey;
        private readonly string serverKey;

        public MidtransPaymentClient(IConfiguration configuration)
        {
            _configuration = configuration;

            isProduction = bool.Parse(_configuration["Midtrans:IsProduction"] ?? "false");
            clientKey = _configuration["Midtrans:ClientKey"] ?? "";
            serverKey = _configuration["Midtrans:ServerKey"] ?? "";
        }

        public string GetAuthToken()
        {
            byte[] authToken = Encoding.UTF8.GetBytes($"{serverKey}:");
            return Convert.ToBase64String(authToken);
        }

        public string GetHost()
        {
            if (isProduction)
                return "https://api.midtrans.com";
            return "https://api.sandbox.midtrans.com";
        }
    }
}

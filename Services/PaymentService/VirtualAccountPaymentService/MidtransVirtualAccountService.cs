using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GymMembership.Data;
using GymMembership.Enums;
using GymMembership.Exceptions;
using GymMembership.Models;

namespace GymMembership.Services
{
    public class MidtransVirtualAccountService : IVirtualAccountPaymentService
    {
        private readonly MidtransPaymentClient _client;
        private readonly HttpClient _httpClient;

        public MidtransVirtualAccountService(MidtransPaymentClient client)
        {
            _client = client;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _client.GetAuthToken());
        }

        public async Task<string> CreatePayment(User user, CreatePaymentData order)
        {
            if (order.PaymentMethod == PaymentMethodEnum.VA_BCA)
                return await CreateBCAVirtualAccount(user, order);
            else if (order.PaymentMethod == PaymentMethodEnum.VA_BNI)
                return await CreateBCAVirtualAccount(user, order);
            
            throw new HttpException("Unknown payment method");
        }

        public async Task<string> CreateBCAVirtualAccount(User user, CreatePaymentData order)
        {
            StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    payment_type = "bank_transfer",
                    transaction_details = new { order_id = order.OrderId, gross_amount = order.Price },
                    bank_transfer = new { bank = "bca" }
                }),
                Encoding.UTF8,
                "application/json"
            );

            Console.WriteLine("Start request");
            Console.WriteLine(jsonContent.ReadAsStringAsync().Result);

            string url = _client.GetHost() + "/v2/charge";
            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);
            // response.EnsureSuccessStatusCode();

            MidtransBCAVAResponseData? jsonResponse = await response.Content.ReadFromJsonAsync<MidtransBCAVAResponseData>();
            Console.WriteLine(jsonResponse);
            Console.WriteLine(jsonResponse.TransactionId);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return "";
        }

        public async Task<string> CreateBNIVirtualAccount(User user, CreatePaymentData order)
        {
            StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    payment_type = "bank_transfer",
                    transaction_details = new { order_id = order.OrderId, gross_amount = order.Price },
                    bank_transfer = new { bank = "bca" }
                }),
                Encoding.UTF8,
                "application/json"
            );

            string url = _client.GetHost() + "/v2/charge";
            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();

            MidtransBCAVAResponseData? jsonResponse = await response.Content.ReadFromJsonAsync<MidtransBCAVAResponseData>();
            Console.WriteLine(jsonResponse);
            Console.WriteLine(jsonResponse.TransactionId);
            return "";
        }
    }
}
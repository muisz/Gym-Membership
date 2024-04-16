using System.Text.Json.Serialization;

namespace GymMembership.Data
{
    public class MidtransBCAVAResponseData
    {
        public string? StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        [JsonPropertyName("transaction_id")]
        public string? TransactionId { get; set; }
        public string? OrderId { get; set; }
        public string? MerchantId { get; set; }
        public string? GrossAmount { get; set; }
        public string? Currency { get; set; }
        public string? PaymentType { get; set; }
        public string? TransactionTime { get; set; }
        public string? TransactionStatus { get; set; }
        public ICollection<MidtransVANumberData> VaNumbers { get; set; } = new List<MidtransVANumberData>();
        public string? FraudStatus { get; set; }
    }
}

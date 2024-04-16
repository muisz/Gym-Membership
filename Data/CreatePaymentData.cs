using GymMembership.Enums;

namespace GymMembership.Data
{
    public class CreatePaymentData
    {
        public string OrderId { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public float Price { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}

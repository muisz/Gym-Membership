using GymMembership.Data;
using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IPaymentService
    {
        public Task<string> CreatePayment(User user, CreatePaymentData order);
    }
}
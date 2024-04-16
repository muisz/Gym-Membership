using GymMembership.Data;
using GymMembership.Models;

namespace GymMembership.Services
{
    public interface IVirtualAccountPaymentService : IPaymentService
    {
        public Task<string> CreateBCAVirtualAccount(User user, CreatePaymentData order);
        public Task<string> CreateBNIVirtualAccount(User user, CreatePaymentData order);
    }
}
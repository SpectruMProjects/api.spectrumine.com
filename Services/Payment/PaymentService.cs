using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Database.CRUDs;

namespace SpectruMineAPI.Services.Payment
{
    public class PaymentService
    {
        private readonly PaymentData PaymentData;
        private readonly ICRUD<User> Users;
        public PaymentService(IOptions<PaymentData> paymentData, ICRUD<User> users) 
        {
            PaymentData = paymentData.Value;
            Users = users;
        }
        public async Task<Errors> RegisterPayment(string signature, int code, string userid, string productid)
        {
            if(code != 1)
            {
                return Errors.Failed;
            }
            var user = await Users.GetAsync(x => x.Id == userid);
            if (user == null)
            {
                return Errors.UserNotFound;
            }
            try
            {
                user.Inventory.Add(ObjectId.Parse(productid));
            }catch (Exception)
            {
                return Errors.ProductNotFound;
            }
            await Users.UpdateAsync(userid, user);
            return Errors.Success;
        }

        public enum Errors { Success, UserNotFound, ProductNotFound, Failed }
    }
}

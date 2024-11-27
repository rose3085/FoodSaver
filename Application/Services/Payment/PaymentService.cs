using Application.DTO.Payment;
using Application.Interfaces.Data;
using Application.Interfaces.Payment;
using Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;

        public PaymentService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<PaymentModel> AddPaymentInfoAsync(PaymentRequestDto paymentRequest)
        {
            try
            {

                if (paymentRequest != null)
                {
                    var id = Guid.NewGuid().ToString();
                    var requestModel = new PaymentModel()
                    {
                        Id = id,
                        PidX = paymentRequest.PidX,
                        Amount = paymentRequest.Amount,
                    };
                    var request = await _uow.AsyncRepositories<PaymentModel>().AddAsync(requestModel);
                    _uow.save();
                    return requestModel;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}

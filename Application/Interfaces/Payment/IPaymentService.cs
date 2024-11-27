using Application.DTO.Payment;
using Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<PaymentModel> AddPaymentInfoAsync(PaymentRequestDto paymentRequest);
    }
}

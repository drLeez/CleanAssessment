using CleanAssessment.DB.Models;
using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.Customer.Queries;
using CleanAssessment.Shared.Bases;
using CleanAssessment.Shared.Tools;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.PaymentMethod.Queries
{
    public class GetAllPaymentMethodsQuery : IRequest<Result<List<PaymentMethodResponse>>>
    {
        public int CustomerId { get; set; }
        public GetAllPaymentMethodsQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }

    internal class GetAllPaymentMethodsHandler : IRequestHandler<GetAllPaymentMethodsQuery, Result<List<PaymentMethodResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllPaymentMethodsHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PaymentMethodResponse>>> Handle(GetAllPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string query = $"EXEC GetPaymentMethods {request.CustomerId}";

                var data = _unitOfWork.StoredProcOrFunction<DB.Models.PaymentMethod>(query).Result
                    .Select(
                        x => new PaymentMethodResponse()
                        {
                            PaymentMethodId = x.PaymentMethodId,
                            NickName = x.NickName,
                            PaymentMethodTypeId = x.PaymentMethodTypeId,
                            OwnerId = x.OwnerId,
                            ExpirationDate = DateTimeTools.FromDateId(x.ExpirationDateId),
                            PaymentMethodTypeCode = x.PaymentMethodTypeCode,
                            PaymentMethodTypeDesc = x.PaymentMethodTypeDesc,
                        }
                    ).ToList();

                return await Result<List<PaymentMethodResponse>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {
                return await Result<List<PaymentMethodResponse>>.FailAsync(ex.Message);
            }
        }
    }
}

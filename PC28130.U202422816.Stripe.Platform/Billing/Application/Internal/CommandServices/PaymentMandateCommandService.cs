using PC2.U202422816.Stripe.Platform.Billing.Application.CommandServices;
using PC2.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC2.U202422816.Stripe.Platform.Billing.Domain.Repositories;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Commands;
using PC28130.U202422816.Stripe.Platform.Shared.Application.Model;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.Errors;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Repositories;

namespace PC2.U202422816.Stripe.Platform.Billing.Application.Internal.CommandServices;

public class PaymentMandateCommandService(
    IPaymentMandateRepository paymentMandateRepository,
    IUnitOfWork unitOfWork)
    : IPaymentMandateCommandService
{
    /// <inheritdoc />
    public async Task<Result<PaymentMandate>> Handle(
        CreatePaymentMandateCommand command,
        CancellationToken cancellationToken)
    {
        var mandateReference = new StripeReference(command.MandateReference);

        var alreadyExists = await paymentMandateRepository
            .ExistsByMandateReferenceAsync(
                mandateReference,
                cancellationToken);

        if (alreadyExists)
        {
            var error = Error.Conflict(
                "PaymentMandate.DuplicateReference",
                "A payment mandate with this reference already exists.");

            return Result<PaymentMandate>.Failure(error);
        }

        var merchantId = new AccountId(command.MerchantId);

        var validityWindow = new EffectiveWindow(
            command.ValidityStartDate,
            command.ValidityEndDate);

        var authorizedLimit = new MonetaryAmount(
            command.AuthorizedLimitValue,
            command.AuthorizedLimitCurrency);

        var paymentMandate = new PaymentMandate(
            mandateReference,
            merchantId,
            validityWindow,
            authorizedLimit,
            command.Notes,
            command.Status);

        await paymentMandateRepository.AddAsync(
            paymentMandate,
            cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        return Result<PaymentMandate>.Success(paymentMandate);
    }
}
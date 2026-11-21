using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Repositories;

namespace PC28130.U202422816.Stripe.Platform.Billing.Domain.Repositories;

public interface IPaymentMandateRepository : IBaseRepository<PaymentMandate>
{
    Task<bool> ExistsByMandateReferenceAsync(StripeReference mandateReference,CancellationToken cancellationToken = default);
}

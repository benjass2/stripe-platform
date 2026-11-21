using Microsoft.EntityFrameworkCore;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Repositories;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace PC28130.U202422816.Stripe.Platform.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PaymentMandateRepository(AppDbContext context)
    : BaseRepository<PaymentMandate>(context), IPaymentMandateRepository
{
    /// <inheritdoc />
    public async Task<bool> ExistsByMandateReferenceAsync(
        StripeReference mandateReference,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<PaymentMandate>()
            .AnyAsync(
                mandate => mandate.MandateReference.Value == mandateReference.Value,
                cancellationToken);
    }
}

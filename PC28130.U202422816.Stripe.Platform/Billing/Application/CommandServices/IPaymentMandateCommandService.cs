using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Commands;
using PC28130.U202422816.Stripe.Platform.Shared.Application.Model;

namespace PC28130.U202422816.Stripe.Platform.Billing.Application.CommandServices;

public interface IPaymentMandateCommandService
{
    Task<Result<PaymentMandate>> Handle(CreatePaymentMandateCommand command, CancellationToken cancellationToken);
}

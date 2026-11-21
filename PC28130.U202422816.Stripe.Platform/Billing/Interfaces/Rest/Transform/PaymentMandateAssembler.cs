using System.Globalization;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Commands;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest.Resources;

namespace PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest.Transform;

/// <summary>Converts API resources to commands and aggregates to resources.</summary>
/// <remarks>Author: Benjamin Solorzano.</remarks>
public static class PaymentMandateAssembler
{
    /// <summary>Builds a command from a validated request.</summary>
    public static CreatePaymentMandateCommand ToCommand(CreatePaymentMandateResource resource) => new(
        resource.MandateReference!.Value, resource.MerchantId!.Value,
        resource.ValidityStartDate!.Value, resource.ValidityEndDate!.Value,
        resource.AuthorizedLimitValue!.Value, resource.AuthorizedLimitCurrency!,
        resource.Notes, resource.Status ?? MandateStatus.Pending);

    /// <summary>Builds the response from a persisted mandate.</summary>
    public static PaymentMandateResource ToResource(PaymentMandate mandate) => new(
        mandate.Id, mandate.MandateReference.Value.ToString(), mandate.MerchantId.Value.ToString(),
        mandate.ValidityWindow.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        mandate.ValidityWindow.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        mandate.AuthorizedLimit.Value, mandate.AuthorizedLimit.Currency);
}

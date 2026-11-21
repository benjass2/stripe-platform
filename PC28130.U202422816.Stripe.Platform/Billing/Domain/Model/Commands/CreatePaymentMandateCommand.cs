using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects; 

namespace PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Commands;

/// <summary>
/// Represents a request to create a payment mandate.
/// </summary>
/// <param name="MandateReference">The external mandate reference.</param>
/// <param name="MerchantId">The merchant account identifier.</param>
/// <param name="ValidityStartDate">The first date of the validity period.</param>
/// <param name="ValidityEndDate">The last date of the validity period.</param>
/// <param name="AuthorizedLimitValue">The authorized monetary limit.</param>
/// <param name="AuthorizedLimitCurrency">The currency of the authorized limit.</param>
/// <param name="Notes">Optional notes for the mandate.</param>
/// <param name="Status">The mandate status. Pending is used by default.</param>
/// <remarks>
/// Author: Benjamin Solorzano.
/// </remarks>
public record CreatePaymentMandateCommand(
    Guid MandateReference,
    Guid MerchantId,
    DateOnly ValidityStartDate,
    DateOnly ValidityEndDate,
    decimal AuthorizedLimitValue,
    string AuthorizedLimitCurrency,
    string? Notes = null,
    MandateStatus Status = MandateStatus.Pending);

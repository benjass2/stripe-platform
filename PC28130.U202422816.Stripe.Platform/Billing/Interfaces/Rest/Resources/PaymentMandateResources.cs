using System.ComponentModel.DataAnnotations;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;

namespace PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest.Resources;

/// <summary>Data required to create a payment mandate.</summary>
/// <remarks>Author: Benjamin Solorzano.</remarks>
public record CreatePaymentMandateResource(
    [Required] Guid? MandateReference,
    [Required] Guid? MerchantId,
    [Required] DateOnly? ValidityStartDate,
    [Required] DateOnly? ValidityEndDate,
    [Required] decimal? AuthorizedLimitValue,
    [Required] string? AuthorizedLimitCurrency,
    string? Notes,
    MandateStatus? Status);

/// <summary>Created payment mandate returned by the API.</summary>
/// <remarks>Author: Benjamin Solorzano.</remarks>
public record PaymentMandateResource(
    int Id, string MandateReference, string MerchantId,
    string ValidityStartDate, string ValidityEndDate,
    decimal AuthorizedLimitValue, string AuthorizedLimitCurrency);

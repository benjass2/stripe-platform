using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.Aggregates.Internal;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.Entities;
using PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;

/// <summary>
/// Represents a payment mandate issued for a merchant account.
/// </summary>
/// <remarks>
/// Author: Benjamin Solorzano.
/// </remarks>
public class PaymentMandate : AggregateRoot, IAuditableEntity
{
    /// <summary>
    /// Gets the identifier generated when the mandate is persisted.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the external reference of the mandate.
    /// </summary>
    public StripeReference MandateReference { get; private set; }

    /// <summary>
    /// Gets the merchant account identifier.
    /// </summary>
    public AccountId MerchantId { get; private set; }

    /// <summary>
    /// Gets the validity period of the mandate.
    /// </summary>
    public EffectiveWindow ValidityWindow { get; private set; }

    /// <summary>
    /// Gets the authorized monetary limit.
    /// </summary>
    public MonetaryAmount AuthorizedLimit { get; private set; }

    /// <summary>
    /// Gets the current mandate status.
    /// </summary>
    public MandateStatus Status { get; private set; }

    /// <summary>
    /// Gets the optional notes for the mandate.
    /// </summary>
    public string? Notes { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Creates a payment mandate.
    /// </summary>
    /// <param name="mandateReference">The external mandate reference.</param>
    /// <param name="merchantId">The merchant account identifier.</param>
    /// <param name="validityWindow">The validity period of the mandate.</param>
    /// <param name="authorizedLimit">The authorized monetary limit.</param>
    /// <param name="notes">Optional notes for the mandate.</param>
    /// <param name="status">The mandate status; pending by default.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when a required value object is null.
    /// </exception>
    public PaymentMandate(
        StripeReference mandateReference,
        AccountId merchantId,
        EffectiveWindow validityWindow,
        MonetaryAmount authorizedLimit,
        string? notes = null,
        MandateStatus status = MandateStatus.Pending)
    {
        ArgumentNullException.ThrowIfNull(mandateReference);
        ArgumentNullException.ThrowIfNull(merchantId);
        ArgumentNullException.ThrowIfNull(validityWindow);
        ArgumentNullException.ThrowIfNull(authorizedLimit);

        MandateReference = mandateReference;
        MerchantId = merchantId;
        ValidityWindow = validityWindow;
        AuthorizedLimit = authorizedLimit;
        Status = status;
        Notes = notes;
    }

    /// <summary>
    /// Creates an instance for Entity Framework.
    /// </summary>
    private PaymentMandate()
    {
        MandateReference = null!;
        MerchantId = null!;
        ValidityWindow = null!;
        AuthorizedLimit = null!;
    }
}

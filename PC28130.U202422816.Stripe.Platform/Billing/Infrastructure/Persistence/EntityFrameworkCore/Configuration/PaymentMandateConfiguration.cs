using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PC2.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.Aggregates;

namespace PC2.U202422816.Stripe.Platform.Billing.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class PaymentMandateConfiguration : IEntityTypeConfiguration<PaymentMandate>
{
    /// <summary>
    /// Configures the payment mandate table and its columns.
    /// </summary>
    /// <param name="builder">The entity configuration builder.</param>
    public void Configure(EntityTypeBuilder<PaymentMandate> builder)
    {
        builder.ToTable("payment_mandates");

        builder.HasKey(mandate => mandate.Id);

        builder.Property(mandate => mandate.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.OwnsOne(mandate => mandate.MandateReference, reference =>
        {
            reference.Property(value => value.Value)
                .HasColumnName("mandate_reference")
                .IsRequired();

            reference.HasIndex(value => value.Value)
                .IsUnique();
        });

        builder.Navigation(mandate => mandate.MandateReference)
            .IsRequired();

        builder.OwnsOne(mandate => mandate.MerchantId, merchant =>
        {
            merchant.Property(value => value.Value)
                .HasColumnName("merchant_id")
                .IsRequired();
        });

        builder.Navigation(mandate => mandate.MerchantId)
            .IsRequired();

        builder.OwnsOne(mandate => mandate.ValidityWindow, window =>
        {
            window.Property(value => value.StartDate)
                .HasColumnName("validity_start_date")
                .IsRequired();

            window.Property(value => value.EndDate)
                .HasColumnName("validity_end_date")
                .IsRequired();
        });

        builder.Navigation(mandate => mandate.ValidityWindow)
            .IsRequired();

        builder.OwnsOne(mandate => mandate.AuthorizedLimit, amount =>
        {
            amount.Property(value => value.Value)
                .HasColumnName("authorized_limit_value")
                .HasPrecision(18, 2)
                .IsRequired();

            amount.Property(value => value.Currency)
                .HasColumnName("authorized_limit_currency")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.Navigation(mandate => mandate.AuthorizedLimit)
            .IsRequired();

        builder.Property(mandate => mandate.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(MandateStatus.Pending)
            .IsRequired();

        builder.Property(mandate => mandate.Notes)
            .HasColumnName("notes");

        builder.Property(mandate => mandate.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(mandate => mandate.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Ignore(mandate => mandate.DomainEvents);
    }
}
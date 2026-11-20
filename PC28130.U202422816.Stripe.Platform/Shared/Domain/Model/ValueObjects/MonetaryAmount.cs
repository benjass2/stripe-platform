namespace PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.ValueObjects;

public record MonetaryAmount
{
    public decimal Value { get; }
    public string Currency { get; }

    public MonetaryAmount(decimal value, string currency)
    {
        if (value < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "The monetary value must be greater than or equal to zero.");
        }
        
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentNullException(
                nameof(currency),
                "The currency must not be null or blank.");
        }
        
        Value = value;
        Currency = currency;
    }

    public MonetaryAmount Multiply(decimal factor)
    {
        return new MonetaryAmount(Value * factor, Currency);
    }

}
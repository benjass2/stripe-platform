namespace PC28130.U202422816.Stripe.Platform.Shared.Domain.Model.ValueObjects;

public record EffectiveWindow
{
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }

    public EffectiveWindow(DateOnly startDate, DateOnly endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be greater than start date.",nameof(endDate));
        }
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool IsExpired(DateOnly checkDate)
    {
        return checkDate > EndDate;
    }
}
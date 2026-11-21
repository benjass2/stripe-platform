using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MySql.Data.MySqlClient;
using PC28130.U202422816.Stripe.Platform.Billing.Application.CommandServices;
using PC28130.U202422816.Stripe.Platform.Billing.Domain.Model.ValueObjects;
using PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest.Resources;
using PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest.Transform;

namespace PC28130.U202422816.Stripe.Platform.Billing.Interfaces.Rest;

/// <summary>Creates payment mandates.</summary>
/// <remarks>Author: Benjamin Solorzano.</remarks>
[ApiController]
[Route("api/v1/payment-mandates")]
[Produces("application/json")]
public class PaymentMandatesController(IPaymentMandateCommandService service, IStringLocalizer<ApiMessages> messages) : ControllerBase
{
    /// <summary>Creates a payment mandate.</summary>
    /// <param name="resource">The mandate data.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>The created mandate or a localized error.</returns>
    [HttpPost]
    [EndpointSummary("Create a payment mandate")]
    [EndpointDescription("Creates a payment mandate with a unique external reference.")]
    [ProducesResponseType<PaymentMandateResource>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMandateResource resource, CancellationToken cancellationToken)
    {
        if (resource.ValidityEndDate <= resource.ValidityStartDate)
            return ErrorResponse(400, "InvalidDates");
        if (resource.AuthorizedLimitValue < 0)
            return ErrorResponse(400, "InvalidAmount");
        if (string.IsNullOrWhiteSpace(resource.AuthorizedLimitCurrency))
            return ErrorResponse(400, "InvalidCurrency");
        if (resource.AuthorizedLimitCurrency.Length > 10)
            return ErrorResponse(400, "InvalidCurrencyLength");
        if (resource.Status.HasValue && !Enum.IsDefined(resource.Status.Value))
            return ErrorResponse(400, "InvalidStatus");
        try
        {
            var result = await service.Handle(PaymentMandateAssembler.ToCommand(resource), cancellationToken);
            if (result.IsFailure) return ErrorResponse(409, "DuplicateReference");
            return StatusCode(StatusCodes.Status201Created, PaymentMandateAssembler.ToResource(result.Value!));
        }
        catch (DbUpdateException ex) when (ex.InnerException is MySqlException { Number: 1062 })
        {
            return ErrorResponse(409, "DuplicateReference");
        }
    }

    private ObjectResult ErrorResponse(int status, string key) =>
        StatusCode(status, new ProblemDetails { Status = status, Title = messages[key], Detail = messages[key] });
}

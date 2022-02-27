using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaymentGateway.APIs.SwaggerExamples;
using PaymentGateway.ApplicationServices.Requests;
using PaymentGateway.ApplicationServices.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.APIs.Controllers
{
    [ApiController]
    [Route("merchants/{merchantId:long}/payments")]
    [Produces("application/json")]
    public class MerchantPaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchantPaymentsController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [HttpGet("{paymentId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(SwaggerResponseExamples.GetMerchantPaymentByIdQueryBadRequestResponseExample))]
        public async Task<ActionResult<GetMerchantPaymentByIdQueryResponse>> GetMerchantPaymentByIdAsync(
            [FromRoute] long merchantId, long paymentId)
        {
            var response = await _mediator.Send(new GetMerchantPaymentByIdQuery(merchantId, paymentId));

            return response == null
                ? NotFound(new GetMerchantPaymentByIdQueryResponse().NotFoundResponse(merchantId, paymentId))
                : Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(SwaggerResponseExamples.NewPaymentCommandBadRequestResponseExample))]
        public async Task<ActionResult<NewPaymentCommandResponse>> ProcessAsync([FromBody] NewPaymentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}

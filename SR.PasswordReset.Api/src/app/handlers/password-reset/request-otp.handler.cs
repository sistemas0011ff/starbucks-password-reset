
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using SR.PasswordReset.Api.src.app.handlers.password_reset.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.application.services;
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions;
using SR.PasswordReset.Api.src.contexts.shared.responses;
using Microsoft.Extensions.Logging;

namespace SR.PasswordReset.Api.src.app.handlers.password_reset
{
    public class RequestOtpHandler
    {
        private readonly IRequestOtpService _requestOtpService;
        private readonly ILogger<RequestOtpHandler> _logger;

        public RequestOtpHandler(
            IRequestOtpService requestOtpService,
            ILogger<RequestOtpHandler> logger)
        {
            _requestOtpService = requestOtpService;
            _logger = logger;
        }

        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Post, "/password-reset/request")]
        public async Task<IHttpResult> RequestOtpHandle([FromBody] RequestOtpRequestDto request, ILambdaContext context)
        {
            _logger.LogInformation("Procesando solicitud para: {Email}", request.Email);

            var applicationDto = new RequestOtpApplicationDto(request.Email);
            var result = await _requestOtpService.Execute(applicationDto);

            return result.IsSuccess
                ? HttpResults.Ok(new BaseResponse(
                    result.Code,
                    result.UserMessage,
                    result.DetailMessage,
                    result.IsSuccess))
                : HttpResults.BadRequest(new ErrorResponseDto(
                    result.Code,
                    result.UserMessage,
                    result.DetailMessage));
        }
    }
}

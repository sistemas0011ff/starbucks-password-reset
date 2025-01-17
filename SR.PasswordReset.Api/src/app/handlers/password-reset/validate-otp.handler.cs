


using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using SR.PasswordReset.Api.src.app.handlers.password_reset.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services;
using SR.PasswordReset.Api.src.contexts.shared.responses;

namespace SR.PasswordReset.Api.src.app.handlers.password_reset
{
    public class ValidateOtpHandler
    {
        private readonly IValidateOtpService _validateOtpService;
        private readonly ILogger<ValidateOtpHandler> _logger;

        public ValidateOtpHandler(
            IValidateOtpService validateOtpService,
            ILogger<ValidateOtpHandler> logger)
        {
            _validateOtpService = validateOtpService;
            _logger = logger;
        }

        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Post, "/password-reset/validate")]
        public async Task<IHttpResult> ValidateOtpHandle([FromBody] ValidateOtpRequestDto request, ILambdaContext context)
        {
            _logger.LogInformation("Procesando validación de OTP para: {Email}", request.Email);

            var applicationDto = new ValidateOtpApplicationDto(request.Email, request.Code);
            var result = await _validateOtpService.Execute(applicationDto);

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

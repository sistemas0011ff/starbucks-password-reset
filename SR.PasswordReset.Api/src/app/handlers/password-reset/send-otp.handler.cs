
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
    public class SendOtpHandler
    {
        private readonly ISendOtpService _sendOtpService;
        private readonly ILogger<SendOtpHandler> _logger;

        public SendOtpHandler(
            ISendOtpService sendOtpService,
            ILogger<SendOtpHandler> logger)
        {
            _sendOtpService = sendOtpService;
            _logger = logger;
        }

        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Post, "/password-reset/send-otp")]
        public async Task<IHttpResult> SendOtpHandle([FromBody] SendOtpRequestDto request, ILambdaContext context)
        {
            _logger.LogInformation("Procesando solicitud de OTP para: {Email}", request.Email);

            var applicationDto = new SendOtpApplicationDto(request.Email);
            var result = await _sendOtpService.Execute(applicationDto);

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

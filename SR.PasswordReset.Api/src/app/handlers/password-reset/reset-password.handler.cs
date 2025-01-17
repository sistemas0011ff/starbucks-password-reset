
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
    public class ResetPasswordHandler
    {
        private readonly IResetPasswordService _resetPasswordService;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(
            IResetPasswordService resetPasswordService,
            ILogger<ResetPasswordHandler> logger)
        {
            _resetPasswordService = resetPasswordService;
            _logger = logger;
        }

        [LambdaFunction]
        [RestApi(LambdaHttpMethod.Post, "/password-reset/reset")]
        public async Task<IHttpResult> ResetPasswordHandle([FromBody] ResetPasswordRequestDto request, ILambdaContext context)
        {
            _logger.LogInformation("Procesando solicitud para: {Email}", request.Email);

            var applicationDto = new ResetPasswordApplicationDto(request.Email, request.NewPassword);
            var result = await _resetPasswordService.Execute(applicationDto);

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

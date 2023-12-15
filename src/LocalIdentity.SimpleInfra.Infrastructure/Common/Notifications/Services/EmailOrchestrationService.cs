using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Extensions;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Notifications.Services;

public class EmailOrchestrationService : IEmailOrchestrationService
{
    private readonly IMapper _mapper;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IEmailRenderingService _emailRenderingService;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IEmailHistoryService _emailHistoryService;
    private readonly IUserService _userService;

    public EmailOrchestrationService(
        IMapper mapper,
        IEmailTemplateService emailTemplateService,
        IEmailRenderingService emailRenderingService,
        IEmailSenderService emailSenderService,
        IEmailHistoryService emailHistoryService,
        IUserService userService
    )
    {
        _mapper = mapper;
        _emailTemplateService = emailTemplateService;
        _emailRenderingService = emailRenderingService;
        _emailSenderService = emailSenderService;
        _emailHistoryService = emailHistoryService;
        _userService = userService;
    }

    public async ValueTask<FuncResult<bool>> SendAsync(EmailNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var sendNotificationRequest = async () =>
        {
            var message = _mapper.Map<EmailMessage>(request);

            // get users
            // set receiver phone number and sender phone number
            var senderUser = (await _userService.GetByIdAsync(request.SenderUserId!.Value, cancellationToken: cancellationToken))!;

            var receiverUser = (await _userService.GetByIdAsync(request.ReceiverUserId, cancellationToken: cancellationToken))!;

            message.SendEmailAddress = senderUser.EmailAddress;
            message.ReceiverEmailAddress = receiverUser.EmailAddress;

            // get template
            message.Template = await _emailTemplateService.GetByTypeAsync(request.TemplateType, true, cancellationToken) ??
                               throw new InvalidOperationException($"Invalid template for sending {NotificationType.Email} notification");

            // render template
            await _emailRenderingService.RenderAsync(message, cancellationToken);

            // send message
            await _emailSenderService.SendAsync(message, cancellationToken);

            // save history
            var history = _mapper.Map<EmailHistory>(message);
            await _emailHistoryService.CreateAsync(history, cancellationToken: cancellationToken);

            return history.IsSuccessful;
        };

        var test = await sendNotificationRequest.GetValueAsync();


        return await sendNotificationRequest.GetValueAsync();
    }
}
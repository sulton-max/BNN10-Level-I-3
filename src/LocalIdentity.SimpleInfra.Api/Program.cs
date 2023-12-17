using LocalIdentity.SimpleInfra.Api.Configurations;

// var systemUser = new User
// {
//     Id = Guid.Parse("b852f637-1779-48fe-9add-ea6bce4068de")
// };
//
// var createdUser = new User
// {
//     Id = Guid.Parse("884bf0ad-50fa-4898-bfe7-3439cf040d69"),
//     FirstName = "john"
// };
//
// var welcomeNotificationEvent = new ProcessNotificationEvent
// {
//     SenderUserId = systemUser.Id,
//     ReceiverUserId = createdUser.Id,
//     TemplateType = NotificationTemplateType.WelcomeNotification,
//     Variables = new Dictionary<string, string>
//             {
//                 { NotificationTemplateConstants.UserNamePlaceholder, createdUser.FirstName }
//             }
// };
//
// var test = JsonConvert.SerializeObject(welcomeNotificationEvent);

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();

await app.ConfigureAsync();
await app.RunAsync();
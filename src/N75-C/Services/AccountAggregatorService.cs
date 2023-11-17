using N75_C.Models;
using N75_C.Models.Entities;

namespace N75_C.Services;

// Primary constructor
public class AccountAggregatorService(UserService userService, EmailSenderService emailSenderService)
{
    public async ValueTask<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        // create user
        var createdUser = await userService.CreateAsync(user, true, cancellationToken);

        // queue welcome notification
        await emailSenderService.QueueAsync(user.Id,
            user.EmailAddress,
            "Welcome",
            "Welcome John",
            cancellationToken: cancellationToken);

        // queue verification notification
        return createdUser;
    }
}
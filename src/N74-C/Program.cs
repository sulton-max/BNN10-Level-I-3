using FluentValidation;
using N74_C.Models;
using N74_C.Services;
using N74_C.Validators;

var userToCreate = new User
{
    FirstName = "ab",
    LastName = "23"
};

var userToUpdate = new User
{
    Id = Guid.Parse("31125086-4D30-4BE4-BD49-0639EBD5291B"),
    FirstName = "John",
    LastName = "Doe"
};

var validUserToCreate = new User
{
    FirstName = "John",
    LastName = "Doe"
};

var validUserToUpdate = new User
{
    Id = Guid.Parse("392B23AA-DA4C-4D9F-AF54-B8FFB5882FF3"),
    FirstName = "John",
    LastName = "Doe"
};

var userValidator = new UserValidator(new UserService());

var testA = userValidator.ValidateAsync(userToCreate, options => options.IncludeRuleSets("OnCreate"));
var testB = userValidator.ValidateAsync(userToUpdate, options => options.IncludeRuleSets("OnUpdate"));
var testC = userValidator.ValidateAsync(validUserToCreate, options => options.IncludeRuleSets("OnCreate"));
var testD = userValidator.ValidateAsync(validUserToUpdate, options => options.IncludeRuleSets("OnUpdate"));

Console.WriteLine();
# Identity Infrastructure

- [project setup]()
- [infrastructure]()
- [setup step-by-setp]()

### Simple auth project setup

- open project **Identity.Local.Infrastructure**
- build and run the project
- postman collection [link]()

### Simple jwt authentication infrastructure

![simple jwt authentication infrastructure]()

- **AuthService** responsible for
    * signing up the user
    * signing in the user
    * is not responsible for saving or validating the user
- **TokenGeneratorService** responsible for
    * generates user claims
    * generates jwt token
    * validates token
    * is not responsible for saving token
- **PasswordHasherService** - responsible for
    * hashes given password
    * validates raw password with hash
    * is not responsible for updating the password

## Setup step-by-step

### Installation

1. create empty web API project
2. create all projects for clean architecture
3. install dependencies
   * In **Persistence** project
       * `Microsoft.EntityFrameworkCore.Design`
       * `Microsoft.EntityFrameworkCore.Tools`
       * `Npgsql.EntityFrameworkCore.PostgreSQL`
       * `Microsoft.AspNetCore.Authentication.JwtBearer`
   * In **Application** project
       * `Microsoft.AspNetCore.Authentication.JwtBearer`
   * In **Infrastructure**
       * `BCrypt.Net-Next`
       * `Newtonsoft.Json`
   * In **API** project
       * `Microsoft.EntityFrameworkCore.Design`
4. create all common entity models in **Domain** project
5. create **DbContext** and entity repository base in **Persistence** project

### Users management

1. create **User** entity and entity configuration
2. add to **DbContext** and create migration
3. create **UserService** foundation service and repositories
4. create **UsersController** controller
5. add identity infrastructure configuration extension method, run and test the project

### Sign up process

1. create **SignUpDetails** model with sign up information
2. create **PasswordHasherService**
3. create **AuthAggregationService** and inject **PasswordHasherService**
4. create **SignUpAsync** method that does next
    * look up for user if found - throws exception
    * if not creates user from sign up details
    * hashes user password and saves along with user
5. create **AuthController** with default **api/auth** route
6. create sign up action with **api/auth/sign-up** route
7. inject **AuthAggregationService** into controller and call

### Sign in process

1. create **SignInDetails** model with sign up information
2. create **TokenGeneratorService**
3.  inject **PasswordHasherService** into **AuthAggregationService**
4. create **SignInAsync** method that does next
    * look up for user if not found - throws exception
    * if found, validates password
    * creates jwt token and returns it
5. create sign in action with **api/auth/sign-in** route
6. inject **AuthAggregationService** into controller and call
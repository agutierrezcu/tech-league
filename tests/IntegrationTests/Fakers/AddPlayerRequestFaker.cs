using System.Security.Cryptography;
using Bogus;
using Web.Api.Players.Add;

namespace IntegrationTests.Fakers;

internal sealed class AddPlayerRequestFaker : Faker<AddPlayerRequest>
{
    public AddPlayerRequestFaker()
    {
        static AddPlayerRequest factoryMethod(Faker faker) =>
           new(
               faker.Name.FullName(),
               faker.Name.FirstName(),
               faker.Date.PastDateOnly(20)
           );

        UseSeed(RandomNumberGenerator.GetInt32(123456789, 987654321))
            .CustomInstantiator(factoryMethod);
    }
}


using System.Security.Cryptography;
using Bogus;
using Web.Api.Players.Add;

namespace IntegrationTests.Fakers;

internal sealed class AddPlayerRequestFaker : Faker<AddPlayerRequest>
{
    public AddPlayerRequestFaker()
    {
        UseSeed(RandomNumberGenerator.GetInt32(123456789, 987654321))
            .CustomInstantiator(FactoryMethod);
        return;

        static AddPlayerRequest FactoryMethod(Faker faker) => new(
                faker.Name.FullName(),
                faker.Name.FirstName(),
                faker.Date.PastDateOnly(20)
            );
    }
}

using System.Security.Cryptography;
using Bogus;
using Domain.Clubs;

namespace UnitTests;

internal sealed class ClubFaker : Faker<Club>
{
    public ClubFaker()
    {
        UseSeed(RandomNumberGenerator.GetInt32(123456789, 987654321))
            .CustomInstantiator(FactoryMethod);
        return;

        static Club FactoryMethod(Faker faker) => new()
        {
            Name = faker.Name.FullName(),
            ThreeLettersName = faker.Name.FirstName()[..3],
            AnualBudget = faker.Finance.Amount() + Club.MinimumAnualBudget
        };
    }
}

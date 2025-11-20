using System.Security.Cryptography;
using Bogus;
using Domain.Clubs;
using Web.Api.Clubs.Add;

namespace IntegrationTests.Fakers;

internal sealed class AddClubRequestFaker : Faker<AddClubRequest>
{
    public AddClubRequestFaker()
    {
        static AddClubRequest factoryMethod(Faker faker) =>
            new(
                faker.Company.CompanyName(),
                faker.Name.FirstName()[..3],
                faker.Finance.Amount() + Club.LeagueMinimumBudgetRequired
            );

        UseSeed(RandomNumberGenerator.GetInt32(123456789, 987654321))
            .CustomInstantiator(factoryMethod);
    }
}

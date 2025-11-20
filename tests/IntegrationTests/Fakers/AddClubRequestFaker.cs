using System.Security.Cryptography;
using Bogus;
using Domain.Clubs;
using Web.Api.Clubs.Add;

namespace IntegrationTests.Fakers;

internal sealed class AddClubRequestFaker : Faker<AddClubRequest>
{
    public AddClubRequestFaker()
    {
        UseSeed(RandomNumberGenerator.GetInt32(123456789, 987654321))
            .CustomInstantiator(FactoryMethod);
        return;

        static AddClubRequest FactoryMethod(Faker faker) 
            => new(
                faker.Company.CompanyName(),
                faker.Name.FirstName()[..3],
                faker.Finance.Amount() + Club.MinimumAnualBudget
            );
    }
}

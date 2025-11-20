using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Application.Clubs.Get;
using Application.Clubs.Players.Get;
using Application.Clubs.Projections;
using Application.Notifications;
using Bogus;
using Domain.Clubs;
using Domain.Contracts;
using Domain.DomainEvents;
using Domain.Players;
using FluentAssertions;
using Infrastructure.EventBus;
using IntegrationTests.Fakers;
using IntegrationTests.WebApplicationFactory;
using Microsoft.Extensions.DependencyInjection;
using Web.Api;
using Web.Api.Clubs.Add;
using Web.Api.Players.Add;

namespace IntegrationTests;

public sealed class ApiClubsIntegrationTests(TechLeagueWebApplicationFactory factory)
    : IClassFixture<TechLeagueWebApplicationFactory>
{
    private static readonly Faker Faker = new();

    private static readonly AddClubRequestFaker AddClubRequestFaker = new();

    private static readonly AddPlayerRequestFaker AddPlayerRequestFaker = new();

    [Fact]
    public async Task AddClub_ThenGetIt()
    {
        AddClubRequest addClubRequest = AddClubRequestFaker.Generate(1)[0];

        string? entityId = await AddEntity("clubs", addClubRequest);

        var clubId = ClubId.Parse(entityId, CultureInfo.CurrentCulture);

        GetClubQueryResult? clubResult =
            await factory.CreateClient().GetFromJsonAsync<GetClubQueryResult>($"/api/clubs/{clubId}");

        clubResult.Should().NotBeNull();

        clubResult.Name
            .Should().Be(addClubRequest!.Name);

        clubResult.ThreeLettersName
            .Should().Be(addClubRequest.ThreeLettersName);

        clubResult.AnualBudget
            .Should().Be(addClubRequest.AnualBudget);
    }

    [Fact]
    public async Task AddPlayer()
    {
        AddPlayerRequest addPlayerRequest = AddPlayerRequestFaker.Generate(1)[0];

        await AddEntity("players", addPlayerRequest);
    }

    [Fact]
    public async Task AddClub_ThenAddPlayer_ThenClubSignUpPlayer_ThenPublishPlayerSignedUpDomainEvent()
    {
        // 1- AddClub
        AddClubRequest addClubRequest = AddClubRequestFaker.Generate(1)[0];

        string? entityId = await AddEntity("clubs", addClubRequest);

        var clubId = ClubId.Parse(entityId, CultureInfo.CurrentCulture);

        // 2- AddPlayer
        AddPlayerRequest addPlayerRequest = AddPlayerRequestFaker.Generate(1)[0];

        entityId = await AddEntity("players", addPlayerRequest);

        var playerId = PlayerId.Parse(entityId, CultureInfo.CurrentCulture);

        // 3- SignUpPlayer
        decimal playerAnualSalary = Faker.Finance.Amount() + PlayerContract.MinimumAnualSalary;

        HttpResponseMessage signUpPlayerRespose = await factory.CreateClient().PostAsJsonAsync(
            $"/api/clubs/{clubId}/players",
            new
            {
                PlayerId = playerId,
                ContractDuration = new
                {
                    Start = Faker.Date.FutureDateOnly(),
                    End = Faker.Date.FutureDateOnly(10)
                },
                AnualSalary = playerAnualSalary
            });

        signUpPlayerRespose.EnsureSuccessStatusCode()
            .StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        // 4- GetPlayersByClub (only just created one)
        int pageSize = 5;

        HttpResponseMessage httpResponseMessage =
            await factory.CreateClient().GetAsync($"/api/clubs/{clubId}/players?page=1&pageSize={pageSize}");

        IEnumerable<PlayerByClub>? players =
            await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<PlayerByClub>>();

        players.Should()
            .NotBeNull()
            .And
            .HaveCount(1);

        PlayerByClub? playerByClub = players.FirstOrDefault();

        playerByClub.Should()
            .NotBeNull();

        playerByClub.Id
            .Should().Be(playerId);

        playerByClub.FullName
            .Should().Be(addPlayerRequest.FullName);

        playerByClub.NickName
            .Should().Be(addPlayerRequest.NickName);

        playerByClub.BirthDate
            .Should().Be(addPlayerRequest.BirthDate);

        playerByClub.FullName
            .Should().Be(addPlayerRequest.FullName);

        AssertHeader(PaginationHeaders.TotalCount, "1");
        AssertHeader(PaginationHeaders.TotalPages, "1");
        AssertHeader(PaginationHeaders.PageNumber, "1");
        AssertHeader(PaginationHeaders.PageSize, pageSize.ToString(CultureInfo.CurrentCulture));
        AssertHeader(PaginationHeaders.HasPrevious, "false");
        AssertHeader(PaginationHeaders.HasNext, "false");

        // 5- Handling published domain event
        IExecutionStrategy backgroundExecutionStrategy =
            factory.Services.GetRequiredService<IExecutionStrategy>();

        var assertBackgroundExecutionStrategy = backgroundExecutionStrategy as NoHandlingExecutionStrategy;

        assertBackgroundExecutionStrategy.Should().NotBeNull();

        var playerSignedUpDomainEvent = assertBackgroundExecutionStrategy.DomainEvent as PlayerSignedUpDomainEvent;

        playerSignedUpDomainEvent.Should().NotBeNull();

        assertBackgroundExecutionStrategy.HandlerTypes.Count()
            .Should().Be(2);

        assertBackgroundExecutionStrategy.HandlerTypes
            .Should().Contain(typeof(ClubFinanceStatusDomainEventHandler));

        assertBackgroundExecutionStrategy.HandlerTypes
            .Should().Contain(typeof(EmailNotificationSendingDomainEventHandler));
        return;

        void AssertHeader(string headerName, string shouldBe)
        {
            httpResponseMessage.Headers.TryGetValues(headerName, out IEnumerable<string>? headerValues)
                .Should().BeTrue($"Header {headerName} is not present in response.");

            headerValues!.FirstOrDefault(v => v == shouldBe)
                .Should().NotBeNull($"Header {headerName} expected to be {shouldBe}");
        }
    }

    private async Task<string> AddEntity<TPayload>(string path, TPayload request)
    {
        HttpResponseMessage addClubRespose =
            await factory.CreateClient().PostAsJsonAsync($"/api/{path}", request);

        addClubRespose.EnsureSuccessStatusCode()
            .StatusCode
            .Should().Be(HttpStatusCode.Created);

        addClubRespose.Headers.Contains("Location")
            .Should().BeTrue();

        addClubRespose.Headers.TryGetValues("Location", out IEnumerable<string> values)
            .Should().BeTrue();

        return values!.FirstOrDefault()?.Split('/').LastOrDefault();
    }
}

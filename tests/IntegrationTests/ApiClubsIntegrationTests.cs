using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Application.Clubs.Get;
using Application.Clubs.Players.Get;
using Application.Clubs.Projections;
using Application.Notifications;
using Bogus;
using Domain.Clubs;
using Domain.DomainEvents;
using Domain.Players;
using IntegrationTests.Fakers;
using IntegrationTests.WebApplicationFactory;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web.Api.Clubs.Add;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Background;
using Web.Api.Players.Add;

namespace IntegrationTests;

public sealed class ApiClubsIntegrationTests : IClassFixture<TechLeagueWebApplicationFactory>
{
    private static readonly Faker _faker = new();

    private static readonly AddClubRequestFaker _addClubRequestFaker = new();

    private static readonly AddPlayerRequestFaker _addPlayerRequestFaker = new();

    private readonly TechLeagueWebApplicationFactory _factory;

    public ApiClubsIntegrationTests(TechLeagueWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddClub_ThenGetIt()
    {
        AddClubRequest addClubRequest = _addClubRequestFaker.Generate(1)[0];

        string? entityId = await AddEntity("clubs", addClubRequest);

        ClubId clubId = new(Guid.Parse(entityId));

        GetClubQueryResult? clubResult =
            await _factory.CreateClient().GetFromJsonAsync<GetClubQueryResult>($"/api/clubs/{clubId}");

        clubResult
            .ShouldNotBeNull();

        clubResult.Name
            .ShouldBe(addClubRequest!.Name);

        clubResult.ThreeLettersName
            .ShouldBe(addClubRequest.ThreeLettersName, StringCompareShould.IgnoreCase);

        clubResult.AnualBudget
            .ShouldBe(addClubRequest.AnualBudget);
    }

    [Fact]
    public async Task AddPlayer()
    {
        AddPlayerRequest addPlayerRequest = _addPlayerRequestFaker.Generate(1)[0];

        await AddEntity("players", addPlayerRequest);
    }

    [Fact]
    public async Task AddClub_ThenAddPlayer_ThenClubSignUpPlayer_ThenPublishPlayerSignedUpDomainEvent()
    {
        // 1- AddClub
        AddClubRequest addClubRequest = _addClubRequestFaker.Generate(1)[0];

        string? entityId = await AddEntity("clubs", addClubRequest);

        ClubId clubId = new(Guid.Parse(entityId));

        // 2- AddPlayer
        AddPlayerRequest addPlayerRequest = _addPlayerRequestFaker.Generate(1)[0];

        entityId = await AddEntity("players", addPlayerRequest);

        PlayerId playerId = new(Guid.Parse(entityId!));

        // 3- SignUpPlayer
        decimal playerAnualSalary = _faker.Finance.Amount() + 1000m;

        HttpResponseMessage signUpPlayerRespose = await _factory.CreateClient().PostAsJsonAsync(
            $"/api/clubs/{clubId}/players",
                new
                {
                    PlayerId = playerId,
                    ContractDuration = new
                    { 
                        Start = _faker.Date.FutureDateOnly(1),
                        End = _faker.Date.FutureDateOnly(10),
                    },
                    AnualSalary = playerAnualSalary,
                });

        signUpPlayerRespose.EnsureSuccessStatusCode()
            .StatusCode
                .ShouldBe(HttpStatusCode.NoContent);

        // 4- GetPlayersByClub (only just created one)
        int pageSize = 5;

        HttpResponseMessage httpResponseMessage =
            await _factory.CreateClient().GetAsync($"/api/clubs/{clubId}/players?page=1&pageSize={pageSize}");

        IEnumerable<PlayerByClub>? players =
            await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<PlayerByClub>>();

        players.ShouldNotBeNull();

        players.Count().ShouldBe(1);

        PlayerByClub playerByClub = players.FirstOrDefault()
            .ShouldNotBeNull();

        playerByClub.Id
            .ShouldBe(playerId);

        playerByClub.FullName
            .ShouldBe(addPlayerRequest.FullName);

        playerByClub.NickName
            .ShouldBe(addPlayerRequest.NickName);

        playerByClub.BirthDate
            .ShouldBe(addPlayerRequest.BirthDate);

        playerByClub.FullName
            .ShouldBe(addPlayerRequest.FullName);

        AssertHeader(PaginationHeaders.TotalCount, "1");
        AssertHeader(PaginationHeaders.TotalPages, "1");
        AssertHeader(PaginationHeaders.PageNumber, "1");
        AssertHeader(PaginationHeaders.PageSize, pageSize.ToString(CultureInfo.CurrentCulture));
        AssertHeader(PaginationHeaders.HasPrevious, "false");
        AssertHeader(PaginationHeaders.HasNext, "false");

        void AssertHeader(string headerName, string shouldBe)
        {
            httpResponseMessage.Headers.TryGetValues(headerName, out IEnumerable<string>? headerValues)
                .ShouldBeTrue($"Header {headerName} is not present in response.");

            headerValues.FirstOrDefault(v => v == shouldBe)
                .ShouldNotBeNull($"Header {headerName} expected to be {shouldBe}");
        }

        // 5- Handling published domain event
        IBackgroundExecutionStrategy backgroundExecutionStrategy =
            _factory.Services.GetRequiredService<IBackgroundExecutionStrategy>();

        var assertBackgroundExecutionStrategy = backgroundExecutionStrategy as NoHandlingBackgroundExecutionStrategy;

        assertBackgroundExecutionStrategy.ShouldNotBeNull();

        var playerSignedUpDomainEvent = assertBackgroundExecutionStrategy.DomainEvent as PlayerSignedUpDomainEvent;

        playerSignedUpDomainEvent.ShouldNotBeNull();

        assertBackgroundExecutionStrategy.HandlerTypes.Count()
            .ShouldBe(2);

        assertBackgroundExecutionStrategy.HandlerTypes
            .Contains(typeof(ClubFinanceStatusDomainEventHandler))
                .ShouldBeTrue();

        assertBackgroundExecutionStrategy.HandlerTypes
            .Contains(typeof(EmailNotificationSendingDomainEventHandler))
                .ShouldBeTrue();
    }

    private async Task<string> AddEntity<TPayload>(string path, TPayload request)
    {
        HttpResponseMessage addClubRespose =
            await _factory.CreateClient().PostAsJsonAsync($"/api/{path}", request);

        addClubRespose.EnsureSuccessStatusCode()
            .StatusCode
                .ShouldBe(HttpStatusCode.Created);

        addClubRespose.Headers.Contains("Location")
            .ShouldBeTrue();

        addClubRespose.Headers.TryGetValues("Location", out IEnumerable<string> values)
            .ShouldBeTrue();

        return values.FirstOrDefault()?.Split('/').LastOrDefault();
    }
}


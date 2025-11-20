using System.Security.Cryptography;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Web.Api.Infrastructure;

internal static class EFCoreDataSeeder
{
    public static readonly IdentifierFactory<ClubId> ClubIdGenerator =
        new(() => new(Guid.CreateVersion7()));

    private static readonly IdentifierFactory<PlayerId> _playerIdGenerator =
        new(() => new(Guid.CreateVersion7()));

    private static readonly IdentifierFactory<CoachId> _coachdGenerator =
        new(() => new(Guid.CreateVersion7()));

    private static readonly IdentifierFactory<ContractId> _contractdGenerator = new(() => new(Guid.CreateVersion7()));

    internal static Func<DbContext, bool, CancellationToken, Task> GetSeeder()
    {
        return async (dbContext, _, ct) =>
        {
            DbSet<Club> clubs = dbContext.Set<Club>();

            bool executed = await clubs.AnyAsync(ct);
            if (executed)
            {
                return;
            }

            await AddClubsAsync(clubs, ct);
            await AddPlayersAsync(dbContext.Set<Player>(), ct);
            await AddCoachesAsync(dbContext.Set<Coach>(), ct);
            await AddClubContractsAsync(dbContext, ct);

            await dbContext.SaveChangesAsync(ct);
        };
    }

    private static Task AddClubsAsync(DbSet<Club> clubs, CancellationToken ct)
    {
        int minBudget = (int)Club.LeagueMinimumBudgetRequired;
        int maxBudget = minBudget * 1000;

        return clubs.AddRangeAsync([
            new()
            {
                Id = ClubIdGenerator.Next("019ab09d-2b4e-7642-a3af-86e660bcb1ab"),
                Name = "Fútbol Club Barcelona",
                ThreeLettersName = "BAR",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Id = ClubIdGenerator.Next("019ab09d-667b-73ab-bddc-5decddb4dfc7"),
                Name = "Real Madrid Club de Fútbol",
                ThreeLettersName = "RMA",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Id = ClubIdGenerator.Next(),
                Name = "Club Atlético de Madrid",
                ThreeLettersName = "ATM",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Id = ClubIdGenerator.Next(),
                Name = "Sevilla Fútbol Club",
                ThreeLettersName = "SEV",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Id = ClubIdGenerator.Next(),
                Name = "Real Betis Balompié",
                ThreeLettersName = "BET",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            }],
        ct);
    }

    private static Task AddPlayersAsync(DbSet<Player> players, CancellationToken ct)
    {
        return players.AddRangeAsync([
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Lionel Andrés Messi",
                NickName = "Leo",
                BirthDate = new(1987, 6, 24)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Neymar da Silva Santos Júnior",
                NickName = "Neymar",
                BirthDate = new(1992, 2, 5)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Xavier Hernández Creus",
                NickName = "Xavi",
                BirthDate = new(1992, 2, 5)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Andrés Iniesta Luján",
                NickName = "Iniesta",
                BirthDate = new(1992, 2, 5)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                NickName = "CR7",
                BirthDate = new(1985, 2, 5)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Karim Mostafa Benzema",
                NickName = "Benzema",
                BirthDate = new(1985, 2, 5)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Raúl González Blanco",
                NickName = "Raúl",
                BirthDate = new(1998, 12, 20)
            },
            new()
            {
                Id = _playerIdGenerator.Next(),
                FullName = "Kylian Mbappé Lottin",
                NickName = "Mbappé",
                BirthDate = new(1998, 12, 20)
            }],
        ct);
    }

    private static Task AddCoachesAsync(DbSet<Coach> coaches, CancellationToken ct)
    {
        return coaches.AddRangeAsync([
            new()
            {
                Id = _coachdGenerator.Next(),
                FullName = "Pep Guardiola",
                Experience = 15
            },
            new()
            {
                Id = _coachdGenerator.Next(),
                FullName = "Carlo Ancelotti",
                Experience = 25
            },
            new()
            {
                Id = _coachdGenerator.Next(),
                FullName = "Jürgen Klopp",
                Experience = 20
            },
            new()
            {
                Id = _coachdGenerator.Next(),
                FullName = "Diego Simeone",
                Experience = 17
            }],
        ct);
    }

    private static async Task AddClubContractsAsync(DbContext dbContext, CancellationToken ct)
    {
        ClubIdGenerator.TryGetEarlierGenerated(out ClubId bcnId);
        ClubIdGenerator.TryGetEarlierGenerated(out ClubId rmId);

        int threshold = 3;

        int current = 0;
        ClubId currentClubId = bcnId;

        while (current <= threshold &&
                _playerIdGenerator.TryGetEarlierGenerated(out PlayerId playerId))
        {
            PlayerContract contract = new()
            {
                Id = _contractdGenerator.Next(),
                ClubId = currentClubId,
                PlayerId = playerId,
                Duration = DateRange.From(DateTime.Today, DateTime.Today.AddYears(3)),
                AnualSalary = RandomNumberGenerator.GetInt32(1000, 5000),
            };

            await dbContext.Set<Contract>()
                .AddAsync(contract, ct);

            current++;

            if (current > threshold)
            {
                currentClubId = rmId;
                current = 0;
            }
        }

        _coachdGenerator.TryGetEarlierGenerated(out CoachId guardiolaId);
        _coachdGenerator.TryGetEarlierGenerated(out CoachId ancelotiId);

        CoachContract guardiolaContract = new()
        {
            Id = _contractdGenerator.Next(),
            ClubId = bcnId,
            CoachId = guardiolaId,
            Duration = DateRange.FromTodayTo(DateTime.Today.AddYears(3)),
            AnualSalary = RandomNumberGenerator.GetInt32(1000, 5000),
        };

        await dbContext.Set<Contract>()
            .AddAsync(guardiolaContract, ct);

        CoachContract ancelotiContract = new()
        {
            Id = _contractdGenerator.Next(),
            ClubId = rmId,
            CoachId = ancelotiId,
            Duration = DateRange.FromTodayTo(DateTime.Today.AddYears(3)),
            AnualSalary = RandomNumberGenerator.GetInt32(1000, 5000),
        };

        await dbContext.Set<Contract>()
            .AddAsync(ancelotiContract, ct);
    }

    internal sealed class IdentifierFactory<TStronglyTypedId>(Func<TStronglyTypedId> factory)
        where TStronglyTypedId : IStronglyTyped<Guid>
    {
        private readonly List<TStronglyTypedId> usedIds = [];

        private readonly Queue<TStronglyTypedId> _generated = new();

        internal TStronglyTypedId Next(string value = "")
        {
            TStronglyTypedId id = string.IsNullOrWhiteSpace(value)
                ? factory()
                : (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), Guid.Parse(value));

            _generated.Enqueue(id!);

            return id;
        }

        internal bool TryGetEarlierGenerated(out TStronglyTypedId? id)
        {
            if (_generated.TryDequeue(out id))
            {
                usedIds.Add(id!);

                return true;
            }

            return false;
        }

        internal TStronglyTypedId[] UsedIds => [.. usedIds];
    }
}

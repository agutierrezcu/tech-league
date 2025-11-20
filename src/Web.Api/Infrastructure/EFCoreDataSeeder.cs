using System.Security.Cryptography;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Infrastructure;

internal static class EfCoreDataSeeder
{
    public static readonly List<ClubId> UsedClubsId = [];

    private static ClubId _fcbClubId;

    private static ClubId _rmaClubId;

    private static CoachId _guardiolaCoachId;

    private static CoachId _ancellotiCoachId;

    private static List<Player> _playerList;

    internal static Func<DbContext, bool, CancellationToken, Task> GetSeeder()
    {
        return async (dbContext, _, ct) =>
        {
            DbSet<Club> clubs = dbContext.Set<Club>();

            bool isInitialized = await clubs.AnyAsync(ct);

            if (isInitialized)
            {
                return;
            }

            await AddClubsAsync(clubs, ct);
            await AddPlayersAsync(dbContext.Set<Player>(), ct);
            await AddCoachesAsync(dbContext.Set<Coach>(), ct);
            await AddClubContractsAsync(dbContext, ct);

            await dbContext.BulkSaveChangesAsync(ct);
        };
    }

    private static async Task AddClubsAsync(DbSet<Club> clubs, CancellationToken ct)
    {
        const int minBudget = (int)Club.MinimumAnualBudget;
        const int maxBudget = minBudget * 1000;

        Club[] clubsArray =
        [
            new()
            {
                Name = "Fútbol Club Barcelona",
                ThreeLettersName = "FCB",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Name = "Real Madrid Club de Fútbol",
                ThreeLettersName = "RMA",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Name = "Club Atlético de Madrid",
                ThreeLettersName = "ATM",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Name = "Sevilla Fútbol Club",
                ThreeLettersName = "SEV",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            },
            new()
            {
                Name = "Real Betis Balompié",
                ThreeLettersName = "BET",
                AnualBudget = RandomNumberGenerator.GetInt32(minBudget, maxBudget)
            }
        ];

        _fcbClubId = clubsArray[0].Id;
        _rmaClubId = clubsArray[1].Id;

        UsedClubsId.AddRange(clubsArray.Select(c => c.Id));

        await clubs.AddRangeAsync(clubsArray, ct);
    }

    private static Task AddPlayersAsync(DbSet<Player> players, CancellationToken ct)
    {
        _playerList =
        [
            new Player
            {
                FullName = "Lionel Andrés Messi",
                NickName = "Leo",
                BirthDate = new DateOnly(1987, 6, 24)
            },
            new Player
            {
                FullName = "Neymar da Silva Santos Júnior",
                NickName = "Neymar",
                BirthDate = new DateOnly(1992, 2, 5)
            },
            new Player
            {
                FullName = "Xavier Hernández Creus",
                NickName = "Xavi",
                BirthDate = new DateOnly(1992, 2, 5)
            },
            new Player
            {
                FullName = "Andrés Iniesta Luján",
                NickName = "Iniesta",
                BirthDate = new DateOnly(1992, 2, 5)
            },
            new Player
            {
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                NickName = "CR7",
                BirthDate = new DateOnly(1985, 2, 5)
            },
            new Player
            {
                FullName = "Karim Mostafa Benzema",
                NickName = "Benzema",
                BirthDate = new DateOnly(1985, 2, 5)
            },
            new Player
            {
                FullName = "Raúl González Blanco",
                NickName = "Raúl",
                BirthDate = new DateOnly(1998, 12, 20)
            },
            new Player
            {
                FullName = "Kylian Mbappé Lottin",
                NickName = "Mbappé",
                BirthDate = new DateOnly(1998, 12, 20)
            }
        ];

        return players.AddRangeAsync(_playerList, ct);
    }

    private static Task AddCoachesAsync(DbSet<Coach> coaches, CancellationToken ct)
    {
        Coach[] coachArray =
        [
            new()
            {
                FullName = "Pep Guardiola",
                Experience = 15
            },
            new()
            {
                FullName = "Carlo Ancelotti",
                Experience = 25
            },
            new()
            {
                FullName = "Jürgen Klopp",
                Experience = 20
            },
            new()
            {
                FullName = "Diego Simeone",
                Experience = 17
            }
        ];

        _guardiolaCoachId = coachArray[0].Id;
        _ancellotiCoachId = coachArray[1].Id;

        return coaches.AddRangeAsync(coachArray, ct);
    }

    private static async Task AddClubContractsAsync(DbContext dbContext, CancellationToken ct)
    {
        DateTime today = DateTime.Today;

        for (int i = 0; i < 4; i++)
        {
            PlayerContract contract = new()
            {
                ClubId = _fcbClubId,
                PlayerId = _playerList[i].Id,
                Duration = GetContractDuration(today),
                AnualSalary = GetRandomAnualSalary(PlayerContract.MinimumAnualSalary)
            };

            await dbContext.Set<Contract>().AddAsync(contract, ct);
        }

        for (int i = 4; i < _playerList.Count; i++)
        {
            PlayerContract contract = new()
            {
                ClubId = _rmaClubId,
                PlayerId = _playerList[i].Id,
                Duration = GetContractDuration(today),
                AnualSalary = GetRandomAnualSalary(PlayerContract.MinimumAnualSalary)
            };

            await dbContext.Set<Contract>().AddAsync(contract, ct);
        }

        CoachContract guardiolaContract = new()
        {
            ClubId = _fcbClubId,
            CoachId = _guardiolaCoachId,
            Duration = GetContractDuration(today),
            AnualSalary = GetRandomAnualSalary(CoachContract.MinimumAnualSalary)
        };

        await dbContext.Set<Contract>().AddAsync(guardiolaContract, ct);

        CoachContract ancellotiContract = new()
        {
            ClubId = _rmaClubId,
            CoachId = _ancellotiCoachId,
            Duration = GetContractDuration(today),
            AnualSalary = GetRandomAnualSalary(CoachContract.MinimumAnualSalary)
        };

        await dbContext.Set<Contract>().AddAsync(ancellotiContract, ct);

        static int GetRandomAnualSalary(decimal minimumAnualSalary) 
            => RandomNumberGenerator.GetInt32((int)minimumAnualSalary, (int)(minimumAnualSalary + 25000));

        static DateRange GetContractDuration(DateTime today) 
            => DateRange.Create(today, today.AddYears(3)).Value;
    }
}

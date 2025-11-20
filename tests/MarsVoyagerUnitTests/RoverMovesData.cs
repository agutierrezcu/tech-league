using static MarsVoyager.Inmutable.CardinalPoint;

namespace MarsVoyagerUnitTests;

public class RoverMovesData : TheoryData<int, int, char, string, int, int, char>
{
    public static IEnumerable<object> BenchmarkData()
    {
        return new RoverMovesData();
    }

    public RoverMovesData()
    {
        // lrfb
        Add(2, 3, NORTH, "lf", 1, 3, WEST);
        Add(5, 8, WEST, "lllfff", 5, 11, NORTH);
        Add(11, -3, SOUTH, "llrlrfff", 14, -3, EAST);
        Add(21, -6, EAST, "llrlrbbb", 21, -9, NORTH);
        Add(33, -87, SOUTH, "llrlrbbbrr", 30, -87, WEST);
        Add(55, -34, SOUTH, "llrlrbbbrrrbbb", 52, -37, NORTH);
        Add(98, -98, WEST, "llbllbbbllllblblblblblblblblbbbb", 104, -98, WEST);
        Add(98, -98, WEST, "xyxyxyxyxyxyxyxyxyxyxyxyxyxyxyxy", 130, -98, WEST);
        Add(231, -678, WEST, "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", 263, -678, WEST);
    }
}

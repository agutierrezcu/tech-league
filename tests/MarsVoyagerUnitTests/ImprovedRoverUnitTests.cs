using Shouldly;

using ImprovedRover = MarsVoyager.Inmutable.Rover;

using static MarsVoyager.Inmutable.CardinalPoint;
using static MarsVoyager.Inmutable.Coordinates;

namespace MarsVoyagerUnitTests;

public class ImprovedRoverUnitTests
{
    [Theory]
    [ClassData(typeof(RoverMovesData))]
    public void ReceiveCommad(int x, int y, char pointingAt,
        string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        ImprovedRover sut = new(x, y, From(pointingAt));

        sut.Receive(commandSequence);

        sut.PointingAt.ShouldBe(From(pointingAtResultant));
        sut.Coordinates.ShouldBe(From(resultantX, resultantY));
    }
}

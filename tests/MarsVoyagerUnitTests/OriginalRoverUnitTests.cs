using Shouldly;

using static MarsVoyager.Inmutable.CardinalPoint;
using static MarsVoyager.Inmutable.Coordinates;

namespace MarsVoyagerUnitTests;

public class OriginalRoverUnitTests
{
    [Theory]
    [ClassData(typeof(RoverMovesData))]
    public void ReceiveCommad(int x, int y, char pointingAt,
        string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        MarsVoyager.Original.Rover sut = new(x, y, From(pointingAt).ToString());

        sut.Receive(commandSequence);

        sut.ToString()
            .ShouldBe($"_direction: {pointingAtResultant}, _x: {resultantX}, _y: {resultantY}");
    }
}

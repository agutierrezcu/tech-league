using MarsVoyager.Inmutable;

namespace MarsVoyager;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var rover = new Rover(15, -6, CardinalPoint.North);

        foreach (string commandSequence in args)
        {
            rover.Receive(commandSequence);
        }

        Console.WriteLine(rover);
    }
}

using System;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace MarsVoyager.MutableRecord;

public sealed class Rover
{
    public CardinalPoint PointingAt { get; private set; }

    public Coordinates Coordinates { get; private set; }

    public Rover(int x, int y, CardinalPoint direction)
    {
        Coordinates = Coordinates.From(x, y);
        PointingAt = direction;
    }

    private const int Displacement = 1;

    public void Receive(string commandsSequence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandsSequence);

        foreach (char command in commandsSequence)
        {
            if (RotationCommand.TryFrom(command, out RotationCommand rotation))
            {
                // Rotate Rover
                PointingAt.Rotate(rotation);
            }
            else
            {
                // Displace Rover
                var direction = (Direcction)command;
                if (!DirecctionExtensions.IsDefined(direction))
                {
                    direction = Direcction.Backward;
                }

                Vector vector = new(PointingAt, direction);

                Coordinates.Apply(vector, Displacement);
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Rover)obj);
    }

    private bool Equals(Rover other)
    {
        return PointingAt == other.PointingAt && Coordinates == other.Coordinates;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PointingAt, Coordinates);
    }

    public override string ToString()
    {
        return $"{nameof(PointingAt)}: {PointingAt}, {nameof(Coordinates)}: {Coordinates}";
    }
}

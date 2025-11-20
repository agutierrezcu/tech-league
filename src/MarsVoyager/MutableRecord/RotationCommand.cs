namespace MarsVoyager.MutableRecord;

public sealed record RotationCommand
{
    private const char NONE = '0';
    private const char LEFT = 'l';
    private const char RIGHT = 'r';

    private readonly char _rotation;

    private RotationCommand(char rotation)
    {
        _rotation = rotation;
    }

    public readonly static RotationCommand None = new(NONE);

    public readonly static RotationCommand Left = new(LEFT);

    public readonly static RotationCommand Right = new(RIGHT);

    public override string ToString() => _rotation.ToString();

    public static bool operator ==(RotationCommand a, char b) => a.Equals(b);
    public static bool operator !=(RotationCommand a, char b) => !(a == b);

    public static bool operator ==(char a, RotationCommand b) => b == a;
    public static bool operator !=(char a, RotationCommand b) => !(a == b);

    public bool IsSame(char rotation, out RotationCommand outRotation)
    {
        if (this == rotation)
        {
            outRotation = this;
            return true;
        }

        outRotation = None;
        return false;
    }

    public static bool TryFrom(char command, out RotationCommand rotation)
    {
        return Left.IsSame(command, out rotation) || Right.IsSame(command, out rotation);
    }
}

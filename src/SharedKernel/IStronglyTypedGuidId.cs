namespace SharedKernel;

public interface IStronglyTyped<out TValue>
{
    TValue Value { get; }
}

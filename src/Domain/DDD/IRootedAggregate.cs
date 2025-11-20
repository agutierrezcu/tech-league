namespace Domain.DDD;

public interface IRootedAggregate<out TRoot>
{
    TRoot Root { get; }
}

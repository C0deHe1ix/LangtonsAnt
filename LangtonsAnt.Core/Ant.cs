namespace LangtonsAnt.Core;

public sealed class Ant
{
    private readonly World _world;

    public Ant(World world, Position position, Direction direction)
    {
        _world = world;
        Position = position;
        Direction = direction;
    }

    public Position Position { get; private set; }
    public Direction Direction { get; private set; }

    public AntStep Step()
    {
        throw new NotImplementedException($"{nameof(Ant)}.{nameof(Step)}");
    }

    public IEnumerable<AntStep> GetSteps(int count)
    {
        throw new NotImplementedException($"{nameof(Ant)}.{nameof(GetSteps)}");
    }
}

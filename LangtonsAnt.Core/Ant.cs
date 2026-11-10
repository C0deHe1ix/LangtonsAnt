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
        Position from = Position;
        bool onWhite = _world.GetColor(from) == CellColor.White;

        int turn = onWhite ? 1 : -1;
        Direction nextDirection = (Direction)(((int)Direction + turn + 4) % 4);
        Position to = nextDirection switch
        {
            Direction.North => new Position(from.X, from.Y - 1),
            Direction.East => new Position(from.X + 1, from.Y),
            Direction.South => new Position(from.X, from.Y + 1),
            Direction.West => new Position(from.X - 1, from.Y),
            _ => throw new InvalidOperationException("Неизвестное направление муравья.")
        };

        CellColor fromColorAfter = onWhite ? CellColor.Black : CellColor.White;
        _world.SetColor(from, fromColorAfter);
        Position = to;
        Direction = nextDirection;

        return new AntStep(from, to, nextDirection, fromColorAfter, _world.GetColor(to));
    }

    public IEnumerable<AntStep> GetSteps(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        for (int i = 0; i < count; i++)
        {
            yield return Step();
        }
    }
}

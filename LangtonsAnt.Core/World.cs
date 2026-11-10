namespace LangtonsAnt.Core;

public sealed class World
{
    public CellColor GetColor(Position position)
    {
        throw new NotImplementedException($"{nameof(World)}.{nameof(GetColor)}");
    }

    public void SetColor(Position position, CellColor color)
    {
        throw new NotImplementedException($"{nameof(World)}.{nameof(SetColor)}");
    }
}

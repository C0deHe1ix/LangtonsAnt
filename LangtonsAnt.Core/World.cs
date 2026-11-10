namespace LangtonsAnt.Core;

public sealed class World
{
    private readonly HashSet<Position> _blackCells = new();

    public CellColor GetColor(Position position)
    {
        return _blackCells.Contains(position) ? CellColor.Black : CellColor.White;
    }

    public void SetColor(Position position, CellColor color)
    {
        if (color == CellColor.Black)
        {
            _blackCells.Add(position);
        }
        else
        {
            _blackCells.Remove(position);
        }
    }
}

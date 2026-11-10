namespace LangtonsAnt.Core;

public sealed record AntStep(
    Position From,
    Position To,
    Direction Direction,
    CellColor FromColorAfter,
    CellColor ToColor);

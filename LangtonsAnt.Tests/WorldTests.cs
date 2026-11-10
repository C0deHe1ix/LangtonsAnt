using LangtonsAnt.Core;
using NUnit.Framework;

namespace LangtonsAnt.Tests;

public class WorldTests
{
    [Test]
    public void GetColor_ReturnsWhite_ForAnUntouchedCell()
    {
        // Arrange
        var world = new World();
        var position = new Position(0, 0);

        // Act
        CellColor color = world.GetColor(position);

        // Assert
        Assert.That(color, Is.EqualTo(CellColor.White));
    }

    [Test]
    public void SetColor_SetsOnlyTheSpecifiedCell()
    {
        var world = new World();
        var position = new Position(-2, -3);
        var neighbor = new Position(-1, -3);

        world.SetColor(position, CellColor.Black);

        Assert.Multiple(() =>
        {
            Assert.That(world.GetColor(position), Is.EqualTo(CellColor.Black));
            Assert.That(world.GetColor(neighbor), Is.EqualTo(CellColor.White));
        });
    }

    [Test]
    public void SetColor_CanTurnABlackCellBackToWhite()
    {
        var world = new World();
        var position = new Position(2, 3);
        world.SetColor(position, CellColor.Black);

        world.SetColor(position, CellColor.White);

        Assert.That(world.GetColor(position), Is.EqualTo(CellColor.White));
    }
}

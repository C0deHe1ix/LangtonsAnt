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
}

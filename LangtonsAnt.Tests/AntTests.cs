using LangtonsAnt.Core;
using NUnit.Framework;

namespace LangtonsAnt.Tests;

public class AntTests
{
    [Test]
    public void Constructor_SetsTheStartingPositionAndDirection()
    {
        var start = new Position(-3, 2);

        var ant = new Ant(new World(), start, Direction.West);

        Assert.Multiple(() =>
        {
            Assert.That(ant.Position, Is.EqualTo(start));
            Assert.That(ant.Direction, Is.EqualTo(Direction.West));
        });
    }

    [TestCase(CellColor.White, Direction.North, Direction.East, 1, 0, CellColor.Black)]
    [TestCase(CellColor.White, Direction.East, Direction.South, 0, 1, CellColor.Black)]
    [TestCase(CellColor.White, Direction.South, Direction.West, -1, 0, CellColor.Black)]
    [TestCase(CellColor.White, Direction.West, Direction.North, 0, -1, CellColor.Black)]
    [TestCase(CellColor.Black, Direction.North, Direction.West, -1, 0, CellColor.White)]
    [TestCase(CellColor.Black, Direction.East, Direction.North, 0, -1, CellColor.White)]
    [TestCase(CellColor.Black, Direction.South, Direction.East, 1, 0, CellColor.White)]
    [TestCase(CellColor.Black, Direction.West, Direction.South, 0, 1, CellColor.White)]
    public void Step_TurnsByCellColor_RepaintsAndMoves(
        CellColor startColor,
        Direction startDirection,
        Direction expectedDirection,
        int expectedX,
        int expectedY,
        CellColor expectedColor)
    {
        var world = new World();
        var start = new Position(0, 0);
        world.SetColor(start, startColor);
        var ant = new Ant(world, start, startDirection);
        var destination = new Position(expectedX, expectedY);

        AntStep step = ant.Step();

        Assert.Multiple(() =>
        {
            Assert.That(step, Is.EqualTo(new AntStep(start, destination, expectedDirection, expectedColor, CellColor.White)));
            Assert.That(ant.Position, Is.EqualTo(destination));
            Assert.That(ant.Direction, Is.EqualTo(expectedDirection));
            Assert.That(world.GetColor(start), Is.EqualTo(expectedColor));
            Assert.That(world.GetColor(destination), Is.EqualTo(CellColor.White));
        });
    }

    [Test]
    public void Step_ReportsTheColorOfABlackDestination_AndLeavesItBlack()
    {
        var world = new World();
        var start = new Position(-3, 2);
        var destination = new Position(-3, 1);
        world.SetColor(destination, CellColor.Black);
        var ant = new Ant(world, start, Direction.West);

        AntStep step = ant.Step();

        Assert.Multiple(() =>
        {
            Assert.That(step.To, Is.EqualTo(destination));
            Assert.That(step.ToColor, Is.EqualTo(CellColor.Black));
            Assert.That(world.GetColor(destination), Is.EqualTo(CellColor.Black));
        });
    }

    [Test]
    public void GetSteps_ReturnsTheRequestedTrajectory_IncludingARevisitedCell()
    {
        var world = new World();
        var start = new Position(0, 0);
        var ant = new Ant(world, start, Direction.North);
        AntStep[] expected =
        [
            new(start, new(1, 0), Direction.East, CellColor.Black, CellColor.White),
            new(new(1, 0), new(1, 1), Direction.South, CellColor.Black, CellColor.White),
            new(new(1, 1), new(0, 1), Direction.West, CellColor.Black, CellColor.White),
            new(new(0, 1), start, Direction.North, CellColor.Black, CellColor.Black),
            new(start, new(-1, 0), Direction.West, CellColor.White, CellColor.White)
        ];

        AntStep[] steps = ant.GetSteps(5).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(steps, Is.EqualTo(expected));
            Assert.That(ant.Position, Is.EqualTo(new Position(-1, 0)));
            Assert.That(world.GetColor(start), Is.EqualTo(CellColor.White));
        });
    }

    [Test]
    public void GetSteps_WaitsForEnumeration_AndMovesOncePerElement()
    {
        var world = new World();
        var start = new Position(0, 0);
        var ant = new Ant(world, start, Direction.North);

        IEnumerable<AntStep> steps = ant.GetSteps(3);
        using IEnumerator<AntStep> enumerator = steps.GetEnumerator();

        Assert.Multiple(() =>
        {
            Assert.That(ant.Position, Is.EqualTo(start));
            Assert.That(ant.Direction, Is.EqualTo(Direction.North));
            Assert.That(world.GetColor(start), Is.EqualTo(CellColor.White));
        });

        Assert.That(enumerator.MoveNext(), Is.True);
        var destination = new Position(1, 0);
        Assert.Multiple(() =>
        {
            Assert.That(enumerator.Current.To, Is.EqualTo(destination));
            Assert.That(ant.Position, Is.EqualTo(destination));
            Assert.That(world.GetColor(start), Is.EqualTo(CellColor.Black));
            Assert.That(world.GetColor(destination), Is.EqualTo(CellColor.White));
        });
    }

    [Test]
    public void GetSteps_WithZeroCount_ReturnsAnEmptySequence()
    {
        var world = new World();
        var start = new Position(0, 0);
        var ant = new Ant(world, start, Direction.North);

        AntStep[] steps = ant.GetSteps(0).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(steps, Is.Empty);
            Assert.That(ant.Position, Is.EqualTo(start));
            Assert.That(ant.Direction, Is.EqualTo(Direction.North));
            Assert.That(world.GetColor(start), Is.EqualTo(CellColor.White));
        });
    }

    [Test]
    public void GetSteps_WithNegativeCount_ThrowsDuringEnumeration()
    {
        var ant = new Ant(new World(), new Position(0, 0), Direction.North);

        IEnumerable<AntStep> steps = ant.GetSteps(-1);

        Assert.Throws<ArgumentOutOfRangeException>(() => steps.ToArray());
    }
}

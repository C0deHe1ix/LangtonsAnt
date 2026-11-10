using System.Diagnostics;
using System.Text;
using LangtonsAnt.Core;

namespace LangtonsAnt.Terminal;

internal sealed class TerminalAnimation
{
    private const int Width = 61;
    private const int Height = 19;
    private const int StepIntervalMilliseconds = 30;
    private const int StatusWidth = 48;
    private static readonly string Border = "+" + new string('-', Width) + "+";

    private readonly HashSet<Position> _blackCells = new();
    private readonly bool _interactive = !Console.IsInputRedirected && !Console.IsOutputRedirected;
    private Position _position;
    private Direction _direction;
    private CellColor _currentColor = CellColor.White;
    private int _left;
    private int _top;
    private int _stepNumber;
    private bool _paused;

    public TerminalAnimation(Position position, Direction direction)
    {
        _position = position;
        _direction = direction;
        _left = position.X - Width / 2;
        _top = position.Y - Height / 2;
    }

    public void Play(Func<IEnumerable<AntStep>> getSteps)
    {
        if (CanRedrawInPlace())
        {
            Console.Clear();
        }

        DrawFrame();
        using var steps = getSteps().GetEnumerator();

        while ((!_interactive || WaitForNextStep()) && steps.MoveNext())
        {
            Apply(steps.Current);
            DrawFrame();
        }
    }

    private bool WaitForNextStep()
    {
        var timer = Stopwatch.StartNew();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(intercept: true).Key)
                {
                    case ConsoleKey.Q:
                        return false;
                    case ConsoleKey.Spacebar:
                        _paused = !_paused;
                        DrawFrame();
                        timer.Restart();
                        break;
                    case ConsoleKey.N when _paused:
                        return true;
                }
            }

            if (!_paused && timer.ElapsedMilliseconds >= StepIntervalMilliseconds)
            {
                return true;
            }

            Thread.Sleep(10);
        }
    }

    private void Apply(AntStep step)
    {
        if (step.FromColorAfter == CellColor.Black)
        {
            _blackCells.Add(step.From);
        }
        else
        {
            _blackCells.Remove(step.From);
        }

        _position = step.To;
        _direction = step.Direction;
        _currentColor = step.ToColor;
        _stepNumber++;

        if (_position.X < _left || _position.X >= _left + Width)
        {
            _left = _position.X - Width / 2;
        }

        if (_position.Y < _top || _position.Y >= _top + Height)
        {
            _top = _position.Y - Height / 2;
        }
    }

    private void DrawFrame()
    {
        var frame = new StringBuilder();
        frame.AppendLine(Border);

        for (int row = 0; row < Height; row++)
        {
            frame.Append('|');
            for (int column = 0; column < Width; column++)
            {
                frame.Append(GetSymbol(new Position(_left + column, _top + row)));
            }

            frame.AppendLine("|");
        }

        frame.AppendLine(Border);
        string color = _currentColor == CellColor.Black ? "чёрная" : "белая";
        string pause = _paused ? " | Пауза" : "";
        frame.AppendLine($"Ход {_stepNumber} | Клетка под муравьём: {color}{pause}".PadRight(StatusWidth));
        if (_interactive)
        {
            frame.AppendLine("Пробел — пауза   N — шаг   Q — выход");
        }

        if (CanRedrawInPlace())
        {
            Console.SetCursorPosition(0, 0);
        }

        Console.Write(frame.ToString());
    }

    private char GetSymbol(Position position)
    {
        if (position != _position)
        {
            return _blackCells.Contains(position) ? '#' : '.';
        }

        return _direction switch
        {
            Direction.North => '^',
            Direction.East => '>',
            Direction.South => 'v',
            Direction.West => '<',
            _ => '?'
        };
    }

    private bool CanRedrawInPlace()
    {
        return _interactive
            && Console.WindowWidth > Math.Max(Width + 2, StatusWidth)
            && Console.WindowHeight > Height + 4;
    }
}

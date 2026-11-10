using System.Text;
using LangtonsAnt.Core;
using LangtonsAnt.Terminal;

Console.OutputEncoding = Encoding.UTF8;

if (args.Length > 1 || (args.Length == 1 && args[0] != "--demo"))
{
    Console.Error.WriteLine("Запуск: dotnet run --project LangtonsAnt.Console [-- --demo]");
    return 2;
}

var start = new Position(0, 0);
const Direction startDirection = Direction.North;
var animation = new TerminalAnimation(start, startDirection);

try
{
    if (args.Length == 1)
    {
        animation.Play(Demo.GetSteps);
    }
    else
    {
        var ant = new Ant(new World(), start, startDirection);
        animation.Play(() => ant.GetSteps(1_000));
    }

    return 0;
}
catch (NotImplementedException exception)
{
    Console.Error.WriteLine($"Метод ещё не реализован: {exception.Message}");
    Console.Error.WriteLine("Готовую демонстрацию можно запустить с параметром --demo.");
    return 1;
}

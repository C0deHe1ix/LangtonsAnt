using System.Globalization;
using LangtonsAnt.Core;

namespace LangtonsAnt.Terminal;

internal static class Demo
{
    public static IEnumerable<AntStep> GetSteps()
    {
        using var stream = typeof(Demo).Assembly.GetManifestResourceStream("LangtonsAnt.DemoSteps.csv")
            ?? throw new InvalidOperationException("Не удалось загрузить запись демонстрации.");
        using var reader = new StreamReader(stream);
        reader.ReadLine(); // Заголовок CSV.

        while (reader.ReadLine() is { } line)
        {
            string[] fields = line.Split(',');
            var from = new Position(
                int.Parse(fields[0], CultureInfo.InvariantCulture),
                int.Parse(fields[1], CultureInfo.InvariantCulture));
            var to = new Position(
                int.Parse(fields[2], CultureInfo.InvariantCulture),
                int.Parse(fields[3], CultureInfo.InvariantCulture));

            yield return new AntStep(
                from,
                to,
                Enum.Parse<Direction>(fields[4]),
                Enum.Parse<CellColor>(fields[5]),
                Enum.Parse<CellColor>(fields[6]));
        }
    }
}

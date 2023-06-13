using NoWa.Parser;

namespace NoWa.Converter;

/// <summary>
/// A converter that converts a WAG to CNF.
/// </summary>
public class NoWaConverter
{

    public static void Convert(string path)
    {
        NoWaParser parser = new NoWaParser();
        parser.Parse(path);
    }
}

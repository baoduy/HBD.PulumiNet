namespace HBD.PulumiNet;

public static class Extensions
{
    public static Output<string> ToOutput(this string value) => Output.Create(value);
}
namespace HBD.PulumiNet.Share.Types;

public static class TypeExtensions
{
    public static ConventionArgs MergeWith(this ConventionArgs me, ConventionArgs other)
        => new (me.Prefix ?? other.Prefix, me.Suffix ?? other.Suffix);
}
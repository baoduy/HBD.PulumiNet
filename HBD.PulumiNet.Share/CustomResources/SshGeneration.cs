namespace HBD.PulumiNet.Share.CustomResources;

/// <summary>
/// Using File as Dynamic Resource is not supported in .NET Core.
/// </summary>
public static class SshGeneration
{
    public record Args(string Name);
    public record Result(string PublicKey,string PrivateKey);
    
    public static Result Creator(Args args)
    {
        var publicKeyName = $"{args.Name}-publicKey";
        var privateKeyName = $"{args.Name}-privateKey";
        
        var publicKey = File.ReadAllText($"/Ssh/{publicKeyName}.pub");
        var privateKey = File.ReadAllText($"/Ssh/{privateKeyName}.pub");
        
        return new Result(publicKey, privateKey);
    }
}
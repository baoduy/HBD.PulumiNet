using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.CustomResources;
using HBD.PulumiNet.Share.Types;
using Pulumi;
using Pulumi.Random;

namespace HBD.PulumiNet.Share.Core;

public static class RandomCreator
{
    public enum PassPolicies
    {
        None = 0,
        Monthly = 1,
        Yearly = 2,
    }

    public record RandomOptions(bool LowerCase = true, bool UpperCase = true, bool Digit = true, bool Special = true);

    public record RandomPassArgs(string Name, int Length = 50, PassPolicies Policy = PassPolicies.Yearly,
        RandomOptions? Options = null);

    /// <summary>
    /// Genera random password with 50 characters length.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static RandomPassword CreatePassword(RandomPassArgs args)
    {
        var keepKey = args.Policy switch
        {
            PassPolicies.Monthly => $"{DateTime.Today.Month}.{DateTime.Today.Year}",
            PassPolicies.Yearly => $"{DateTime.Today.Year}",
            _ => string.Empty
        };

        var options = args.Options ?? new RandomOptions();
        var name = args.Name.GetPasswordName();

        return new RandomPassword(name, new RandomPasswordArgs
        {
            Keepers = new InputMap<object> { { "keepKey", keepKey } },
            Length = args.Length,
            Lower = options.LowerCase,
            MinLower = 4,
            Upper = options.UpperCase,
            MinUpper = 4,
            Number = options.Digit,
            MinNumeric = 4,
            Special = options.Special,
            MinSpecial = 4,
            //Exclude some special characters that are not accept4d by XML and SQLServer.
            OverrideSpecial = "#%&*+-/:<>?^_|~",
        });
    }

    public static RandomUuid CreateUuId(string name) => new(name);

    public static RandomString CreateString(string name, int length = 10, RandomOptions? options = null)
    {
        options ??= new RandomOptions();
        return new RandomString(name, new RandomStringArgs
        {
            Keepers = new InputMap<object> { { "length", length } },
            Length = length,
            Lower = options.LowerCase,
            //MinLower = options.LowerCase ? 1 : 0,
            Upper = options.UpperCase,
            //MinUpper = options.UpperCase ? 1 : 0,
            Number = options.Digit,
            //MinNumeric = options.Digit ? 1 : 0,
            Special = options.Special,
            //MinSpecial = options.Special ? 1 : 0,
        });
    }

    public record RandomLoginNameArgs(string Name, int Length = 15, string? Prefix = default,
        RandomOptions? Options = null);

    public static Output<string> CreateLoginName(RandomLoginNameArgs args)
    {
        var name = $"{args.Prefix}{args.Name}";
        var remaining = args.Length - name.Length;

        if (remaining <= 0)
            return Output.Create(name);

        var rd = CreateString(name, remaining, args.Options ?? new RandomOptions { Special = false });
        return Output.Format($"{name}{rd.Result}");
    }
    
    public record SshArgs(string Name,AzResourceInfo? VaultInfo=null);
    
    public static SshGeneration.Result RandomSsh (SshArgs args)
    {
        var name = args.Name.GetSshName();
        var rs= SshGeneration.Creator(new SshGeneration.Args(name));

        if (args.VaultInfo!=null)
        {
            var publicKeyName = $"{args.Name}-publicKey";
            var privateKeyName = $"{args.Name}-privateKey";
            
            
        }

        return rs;
    }
}
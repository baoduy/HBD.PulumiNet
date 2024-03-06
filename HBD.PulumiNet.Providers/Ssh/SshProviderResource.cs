using Ibasa.Pulumi.Dynamic;
using Pulumi;
using Pulumi.Experimental.Provider;

namespace HBD.PulumiNet.Providers.Ssh;

public sealed class SshArgs : DynamicResourceArgs
{
};

public sealed class SshProvider : DynamicProvider
{
    // public override Task<CheckResponse> CheckConfig(CheckRequest request, CancellationToken ct)
    //     => Task.FromResult(new CheckResponse { Inputs = request.NewInputs });
    //
    // public override Task<DiffResponse> DiffConfig(DiffRequest request, CancellationToken ct)
    //     => Task.FromResult(new DiffResponse());
    //
    // public override Task<ConfigureResponse> Configure(ConfigureRequest request, CancellationToken ct)
    //     => Task.FromResult(new ConfigureResponse());
    //
    // public override Task<CheckResponse> Check(CheckRequest request, CancellationToken ct)
    // {
    //     return Task.FromResult(new CheckResponse { Inputs = request.NewInputs });
    // }

    // public override Task<DiffResponse> Diff(DiffRequest request, CancellationToken ct)
    // {
    //     var changes = !request.OldState[nameof(SshArgs.Password)].Equals(request.NewInputs[nameof(SshArgs.Password)]);
    //
    //     return Task.FromResult(new DiffResponse
    //     {
    //         Changes = changes,
    //         DeleteBeforeReplace = true,
    //         Replaces = changes ? new[] { nameof(SshArgs.Password) } : null,
    //     });
    // }

    // private static (string publicKey, string privateKey) GenerateSsh()
    // {
    //     var keygen = new SshKeyGenerator.SshKeyGenerator(2048);
    //
    //     var privateKey = keygen.ToPrivateKey();
    //     var publicKey = keygen.ToRfcPublicKey();
    //
    //     return (publicKey, privateKey);
    // }

    public override Task<CreateResponse> Create(CreateRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateResponse
        {
            Id = Guid.NewGuid().ToString(),
            Properties = new Dictionary<string, PropertyValue> { ["publicKey"] = new("123"), ["privateKey"] = new("456") }
        });
    }

    // public override Task<ReadResponse> Read(ReadRequest request, CancellationToken ct)
    // {
    //     var response = new ReadResponse
    //     {
    //         Id = request.Id,
    //         Properties = request.Properties,
    //     };
    //     return Task.FromResult(response);
    // }
}

public sealed class SshProviderResource(string name, SshArgs? args = null, CustomResourceOptions? options = null)
    : DynamicResource<SshArgs>(Provider, name, args, options, null, nameof(SshProviderResource))
{
    private static readonly SshProvider Provider = new();

    [Output("publicKey")] public Output<string> PublicKey { get; set; } = null!;

    [Output("privateKey")] public Output<string> PrivateKey { get; set; } = null!;
}
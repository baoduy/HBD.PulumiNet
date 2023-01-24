using Pulumi;
using Pulumi.Experimental.Provider;

namespace HBD.PulumiNet.Share.CustomResources.Ssh;

public record SshArgs(string? Password = null);

public class SshProvider : Provider
{
    readonly IHost host;
    private readonly SshArgs _args;

    public SshProvider(IHost host, SshArgs? args = null)
    {
        this.host = host;
        _args = args ?? new SshArgs();
    }

    public override Task<CheckResponse> CheckConfig(CheckRequest request, CancellationToken ct)
        => Task.FromResult(new CheckResponse { Inputs = request.News });

    public override Task<DiffResponse> DiffConfig(DiffRequest request, CancellationToken ct)
        => Task.FromResult(new DiffResponse());

    public override Task<ConfigureResponse> Configure(ConfigureRequest request, CancellationToken ct)
        => Task.FromResult(new ConfigureResponse());

    public override Task<CheckResponse> Check(CheckRequest request, CancellationToken ct)
    {
        return Task.FromResult(new CheckResponse { Inputs = request.News });
    }

    public override Task<DiffResponse> Diff(DiffRequest request, CancellationToken ct)
    {
        var changes = !request.Olds[nameof(SshArgs.Password)].Equals(nameof(SshArgs.Password));

        return Task.FromResult(new DiffResponse
        {
            Changes = changes,
            DeleteBeforeReplace = false,
            Replaces = changes ? new[] { nameof(SshArgs.Password) } : null,
        });
    }

    private static void GenerateSsh(string password)
    {
    }
    

    public override Task<CreateResponse> Create(CreateRequest request, CancellationToken ct)
    {
        return Task.FromResult(new CreateResponse {
            Id = "",
        });
    }

    public override Task<UpdateResponse> Update(UpdateRequest request, CancellationToken ct)
    {
        
        return Task.FromResult(new UpdateResponse {
            Properties = request.News
        });
    }

    public override Task Delete(DeleteRequest request, CancellationToken ct) => Task.CompletedTask;

    public override Task<ReadResponse> Read(ReadRequest request, CancellationToken ct)
    {
        var response = new ReadResponse
        {
            Id = request.Id,
            Properties = request.Properties,
        };
        return Task.FromResult(response);
    }
}
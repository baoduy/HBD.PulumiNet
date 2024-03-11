using System.Threading.Tasks;
using Pulumi;

namespace HBD.PulumiNet.Tests;

public static class TestExtensions
{
    public static Task<T> GetValueAsync<T>(this Output<T> output)
    {
        var tcs = new TaskCompletionSource<T>();
        output.Apply(v => { tcs.SetResult(v); return v; });
        return tcs.Task;
    }
}
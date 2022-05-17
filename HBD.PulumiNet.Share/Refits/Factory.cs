namespace HBD.PulumiNet.Share.Refits;

public static class Factory
{
    private static HttpClient? _client;

    private static async ValueTask<HttpClient> GetOrCreateHttpClient()
    {
        if (_client != null) return _client;

        var info = await Pulumi.AzureNative.Authorization.GetClientConfig.InvokeAsync();
        var token = await Pulumi.AzureNative.Authorization.GetClientToken.InvokeAsync();

        _client = new HttpClient();
        _client.BaseAddress = new Uri($"https://management.azure.com/subscriptions/{info.SubscriptionId}");
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");

        return _client;
    }

    public static async Task<T> Create<T>() 
        => Refit.RestService.For<T>(await GetOrCreateHttpClient());
}
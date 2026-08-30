[![](https://img.shields.io/nuget/v/soenneker.sabnzbd.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.sabnzbd.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.sabnzbd.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.sabnzbd.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.sabnzbd.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.sabnzbd.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.sabnzbd.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.sabnzbd.openapiclient/actions/workflows/codeql.yml)

# Soenneker.Sabnzbd.OpenApiClient

A Kiota-generated .NET client for SABnzbd's HTTP API.

## Installation

```bash
dotnet add package Soenneker.Sabnzbd.OpenApiClient
```

For dependency injection, cached HTTP transport, configuration, and API-key authentication, install the companion utility:

```bash
dotnet add package Soenneker.Sabnzbd.OpenApiClientUtil
```

## Usage with the client utility

```json
{
  "Sabnzbd": {
    "ClientBaseUrl": "http://localhost:8080",
    "ApiKey": "your-api-key"
  }
}
```

```csharp
using Soenneker.Sabnzbd.OpenApiClient;
using Soenneker.Sabnzbd.OpenApiClient.Models;
using Soenneker.Sabnzbd.OpenApiClientUtil.Abstract;
using Soenneker.Sabnzbd.OpenApiClientUtil.Registrars;

services.AddSabnzbdOpenApiClientUtilAsSingleton();

public sealed class SabnzbdQueueReader(ISabnzbdOpenApiClientUtil clientUtil)
{
    public async Task<ApiCommandResponse?> GetQueue(CancellationToken cancellationToken)
    {
        SabnzbdOpenApiClient client = await clientUtil.Get(cancellationToken);

        return await client.Api.GetAsync(request =>
        {
            request.QueryParameters.Mode = Mode.Queue;
            request.QueryParameters.Output = Output.Json;
        }, cancellationToken);
    }
}
```

## Response shape

SABnzbd uses one `/api` endpoint for many commands, selected by the `mode` query parameter. `ApiCommandResponse` is therefore a combined envelope: only the properties relevant to the requested mode are populated. For example, queue calls populate `Queue`, history calls populate `History`, and fields not described by the schema are retained in `AdditionalData`.

The generated client requires a Kiota `IRequestAdapter`. Most SABnzbd commands also require the `apikey` query parameter. The companion `Soenneker.Sabnzbd.OpenApiClientUtil` package configures both and prevents the API key from being attached to a different authority.

Generated APIs and models can change when the upstream specification changes; keep application-specific mapping behind your own service boundary.

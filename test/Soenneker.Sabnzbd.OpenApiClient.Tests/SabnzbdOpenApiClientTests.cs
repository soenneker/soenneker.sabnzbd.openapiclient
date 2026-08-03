using Soenneker.Tests.HostedUnit;

namespace Soenneker.Sabnzbd.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SabnzbdOpenApiClientTests : HostedUnitTest
{
    public SabnzbdOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}

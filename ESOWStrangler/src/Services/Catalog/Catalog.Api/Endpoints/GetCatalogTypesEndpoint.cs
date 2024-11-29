using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using FastEndpoints;

namespace Catalog.Api.Endpoints;

public class GetCatalogTypesEndpoint(IGetCatalogService<CatalogType> getCatalogService)
: Endpoint<GetCatalogTypesRequest, GetCatalogTypesResponse>
{
    public override void Configure()
    {
        Get("/api/catalog/types");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCatalogTypesRequest request, CancellationToken cancellationToken)
    {
        var result = await getCatalogService.GetAllAsync();
        var response = new GetCatalogTypesResponse { Types = result };
        await SendAsync(response);
    }
}

public class GetCatalogTypesRequest
{
    public bool DummyProperty { get; set; } = true;
}

public class GetCatalogTypesResponse
{
    public IEnumerable<CatalogType>? Types { get; set; }
}

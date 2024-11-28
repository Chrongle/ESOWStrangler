using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using FastEndpoints;

namespace Catalog.Api.Endpoints;

public class GetCatalogBrandsEndpoint(IGetCatalogService<CatalogBrand> getCatalogService)
: Endpoint<GetCatalogBrandsRequest, GetCatalogBrandsResponse>
{
    public override void Configure()
    {
        Get("/api/catalog/brands");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCatalogBrandsRequest request, CancellationToken cancellationToken)
    {
        var result = await getCatalogService.GetAllAsync();
        var response = new GetCatalogBrandsResponse { Brands = result };
        await SendAsync(response);
    }
}

public class GetCatalogBrandsRequest
{
    public bool DummyProperty { get; set; } = true;
}

public class GetCatalogBrandsResponse
{
    public IEnumerable<CatalogBrand>? Brands { get; set; }
}

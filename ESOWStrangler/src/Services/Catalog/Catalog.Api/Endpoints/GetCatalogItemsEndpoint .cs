using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using FastEndpoints;

namespace Catalog.Api.Endpoints;

public class GetCatalogItemsEndpoint(IGetCatalogItemService getCatalogService)
: Endpoint<GetCatalogItemsRequest, GetCatalogItemsResponse>
{
    public override void Configure()
    {
        Get("/api/catalog/items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCatalogItemsRequest request, CancellationToken cancellationToken)
    {
        var result = await getCatalogService.GetAllAsync(request.BrandId, request.TypeId);
        var response = new GetCatalogItemsResponse { Items = result };
        await SendAsync(response);
    }
}

public class GetCatalogItemsRequest
{
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
}

public class GetCatalogItemsResponse
{
    public IEnumerable<CatalogItem>? Items { get; set; }
}

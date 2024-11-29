using Catalog.Core.Interfaces;
using FastEndpoints;

namespace Catalog.Api.Endpoints;

public class GetCatalogItemEndpoint(IGetCatalogItemService getCatalogService)
: Endpoint<GetCatalogItemRequest, GetCatalogItemResponse>
{
    public override void Configure()
    {
        Get("/api/catalog/item");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCatalogItemRequest request, CancellationToken cancellationToken)
    {
        var result = await getCatalogService.GetAsync(request.Id);
        if (result.Id == 0) await SendNotFoundAsync();

        var response = new GetCatalogItemResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Price = result.Price,
            PictureUri = result.PictureUri,
            CatalogTypeId = result.CatalogTypeId,
            CatalogBrandId = result.CatalogBrandId
        };
        await SendAsync(response);
    }
}

public class GetCatalogItemRequest
{
    public int Id { get; set; }
}

public class GetCatalogItemResponse
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureUri { get; set; }

    public int? CatalogTypeId { get; set; }

    public int? CatalogBrandId { get; set; }
}

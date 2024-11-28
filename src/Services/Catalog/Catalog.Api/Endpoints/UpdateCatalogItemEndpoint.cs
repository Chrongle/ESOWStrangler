using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using FastEndpoints;

namespace Catalog.Api.Endpoints;

public class UpdateCatalogItemEndpoint(IUpdateCatalogService updateCatalogService)
: Endpoint<UpdateCatalogItemRequest>
{
    public override void Configure()
    {
        Put("/api/catalog/item/update");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCatalogItemRequest request, CancellationToken cancellationToken)
    {
        await updateCatalogService.UpdateItemAsync(new CatalogItem
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            PictureUri = request.PictureUri,
            CatalogTypeId = request.CatalogTypeId,
            CatalogBrandId = request.CatalogBrandId
        });

        await SendOkAsync();
    }
}

public class UpdateCatalogItemRequest
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureUri { get; set; }

    public int? CatalogTypeId { get; set; }

    public int? CatalogBrandId { get; set; }
}

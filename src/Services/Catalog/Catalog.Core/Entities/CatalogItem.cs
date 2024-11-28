using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities;

public partial class CatalogItem : IAggregateRoot
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureUri { get; set; }

    public int? CatalogTypeId { get; set; }

    public int? CatalogBrandId { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities;
public partial class CatalogBrand : IAggregateRoot
{
    [Key]
    public int Id { get; set; }

    public string? BrandName { get; set; }
}

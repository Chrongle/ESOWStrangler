using System.ComponentModel.DataAnnotations;

namespace Order.Core.Entities;

public class ShippingAddress
{
  [Key]
  public int Id { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Address { get; set; }
  public required string ZipCode { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
  public required string Mobile { get; set; }
  public required string Email { get; set; }
}
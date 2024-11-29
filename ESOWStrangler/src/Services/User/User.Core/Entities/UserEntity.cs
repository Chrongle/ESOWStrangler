using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Core.Entities;
public class UserEntity
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
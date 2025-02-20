using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Wpm.Managment.Api.Models;

public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public Guid BreedId { get; set; }
    public Breed Breed { get; set; }
}

public record Breed(Guid Id, string Name);
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Managment.Api.DataAccess;
using Wpm.Managment.Api.Models;

namespace Wpm.Managment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController(ILogger<PetsController> logger, ManagmentDbContext dbContext)
    : ControllerBase
{
    private readonly ILogger<PetsController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetPets()
    {
        logger.LogInformation("Retrieving all the pets");
        return Ok(await dbContext.Pets.Include(x => x.Breed).ToListAsync());
    }
    
    [HttpGet("{Id:guid}", Name = nameof(GetPetById))]
    public async Task<IActionResult> GetPetById(Guid Id)
    {
        _logger.LogInformation($"Retrieving pets for {Id}");
        
        var result = await dbContext.Pets.Include(x => x.Breed)
                                              .FirstOrDefaultAsync(x => x.Id == Id);
        
        if(result == null) return NotFound("Pet not found");
        
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePet([FromBody] NewPet newPet)
    {
        var pet = newPet.ToPet();

        var breed = dbContext.Breeds.Where(x => x.Id == newPet.BreedId);
        
        if(!breed.Any()) return NotFound("Breed not found");
        
        await dbContext.Pets.AddAsync(pet);
        await dbContext.SaveChangesAsync();

        return CreatedAtRoute(nameof(GetPetById), new { id = pet.Id }, newPet);
    }
    
    
    [HttpPut]
    public async Task<IActionResult> UpdatePet([FromQuery] Guid Id, [FromBody] ModifyPet modifyPet)
    {
        var pet = await dbContext.Pets.FindAsync(Id);
        
        if(pet is null) return NotFound("Pet not found");
        
        var findBreed = await dbContext.Breeds.FindAsync(modifyPet.BreedId);
        if(findBreed is null) return NotFound("Breed not found");
        
        pet.Name = modifyPet.Name;
        pet.Age = modifyPet.Age;
        pet.BreedId = modifyPet.BreedId;
        
        await dbContext.SaveChangesAsync();

        return Ok(pet);
    }
    
    [HttpDelete("{Id:guid}")]
    public async Task<IActionResult> DeletePetById(Guid Id)
    {
        _logger.LogInformation($"Processing Pet deletion for Id: {Id}");

        var pet = await dbContext.Pets.FindAsync(Id);
        if(pet is null) return NotFound("Pet not found to delete");

        dbContext.Pets.Remove(pet);
        dbContext.SaveChangesAsync();

        return Ok(pet);
    }
    
    public record NewPet(string Name, int Age, Guid BreedId)
    {
        public Pet ToPet()
        {
            return new Pet() { Name = Name, Age = Age, BreedId = BreedId };
        }
    }

    
    public record ModifyPet(string Name, int Age, Guid BreedId)
    {
        public Pet ToPet()
        {
            return new Pet() { Name = Name, Age = Age, BreedId = BreedId };
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Managment.Api.DataAccess;
using Wpm.Managment.Api.Models;

namespace Wpm.Managment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class BreedController(ILogger<BreedController> logger, ManagmentDbContext dbContext)
    : ControllerBase
{
    private readonly ILogger<BreedController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetBreedsAsync()
    {
        logger.LogInformation("Retrieving all the breeds");
        var result = await dbContext.Breeds.ToListAsync();

        return Ok(result);
    }

    [HttpGet("{Id:guid}", Name = nameof(GetBreedById))]
    public async Task<IActionResult> GetBreedById(Guid Id)
    {
        _logger.LogInformation($"Retrieving breeds for {Id}");

        var result = await dbContext.Breeds
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (result == null) return NotFound("Breed not found");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBreed(string breedName)
    {
        var breed = await dbContext.Breeds.Where(x => x.Name.ToLower() == breedName.ToLower())
            .FirstOrDefaultAsync();
        if (breed is not null) return BadRequest("Breed already exists");

        var newBreed = new Breed(Guid.NewGuid(), breedName);

        await dbContext.Breeds.AddAsync(newBreed);
        await dbContext.SaveChangesAsync();

        return Ok(newBreed);
    }

    [HttpDelete("{Id:guid}")]
    public async Task<IActionResult> DeleteBreedById(Guid Id)
    {
        _logger.LogInformation($"Processing breed deletion for Id: {Id}");

        var checkPetsBreed = await dbContext.Pets.Where(x => x.BreedId == Id).ToListAsync();
        if (checkPetsBreed.Any())
            return BadRequest("Cannot delete breed because is associated with one or more pets");

        var breedToEliminate = await dbContext.Breeds
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (breedToEliminate == null) return NotFound("Breed not found");

        dbContext.Breeds.Remove(breedToEliminate);
        dbContext.SaveChangesAsync();

        return Ok(breedToEliminate);
    }
}
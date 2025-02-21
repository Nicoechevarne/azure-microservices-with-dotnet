namespace Wpm.Clinic.ExternalServices;

public class ManagementService(HttpClient client)
{
    public async Task<PetInfo> GetPetInfo(Guid id)
    {
        var petInfo = await client.GetFromJsonAsync<PetInfo>($"/api/pets/{id}");
        return petInfo;
    }
}

public record PetInfo(Guid Id, string Name, int Age, Guid BreedId);
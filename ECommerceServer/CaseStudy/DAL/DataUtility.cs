using CaseStudy.DAL.DomainClasses;
using System.Text.Json;
namespace CaseStudy.DAL;

public class DataUtility
{
    private readonly AppDbContext _db;
    public DataUtility(AppDbContext context)
    {
        _db = context;
    }

    private async Task<bool> LoadBrands(dynamic jsonObjectArray)
    {
        bool loadedBrands = false;
        try
        {
            // clear out the old rows
            _db.Brands?.RemoveRange(_db.Brands);
            await _db.SaveChangesAsync();
            List<String> allBrands= new();
            foreach (JsonElement element in jsonObjectArray.EnumerateArray())
            {
                if (element.TryGetProperty("BRAND", out JsonElement brandJson))
                {
                    allBrands.Add(brandJson.GetString()!);
                }
            }
            IEnumerable<String> brands = allBrands.Distinct<String>();
            foreach (string braname in brands)
            {
                Brand bra = new();
                bra.Name = braname;
                await _db.Brands!.AddAsync(bra);
                await _db.SaveChangesAsync();
            }
            loadedBrands = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error - " + ex.Message);
        }
        return loadedBrands;
    }

}
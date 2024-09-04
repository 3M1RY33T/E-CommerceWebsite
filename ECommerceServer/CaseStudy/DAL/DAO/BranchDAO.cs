using CaseStudy.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace CaseStudy.DAL.DAO
{
    public class BranchDAO
    {
        private readonly AppDbContext _db;
        public BranchDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public async Task<List<Branch>?> GetThreeClosestBranches(float? lat, float? lon)
        {
            List<Branch>? storeDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat1", lat);
                var lonParam = new SqlParameter("@lng1", lon);

                var query = _db.Branches?.FromSqlRaw("EXEC dbo.pGetThreeClosestBranches @lat1, @lng1", latParam, lonParam);

                storeDetails = await query!.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return storeDetails;
        }
    }
}

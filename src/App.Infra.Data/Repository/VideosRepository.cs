using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public class VideosRepository : IVideosRepository
    {
        private readonly MySQLContext _dbContext;

        public VideosRepository(MySQLContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task PostVideo(VideoBD Video)
        {
            _dbContext.Videos.Add(Video);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<VideoBD> GetById(int id)
        {

            var query = _dbContext.Videos.AsQueryable();
            query = query.Where(c => c.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        

     
    }
}

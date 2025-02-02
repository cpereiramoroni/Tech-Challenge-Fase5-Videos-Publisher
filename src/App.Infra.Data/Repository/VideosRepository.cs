using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public async Task<IList<VideoBD>> GetVideosByIdStatus(int? idStatus)
        {

            var query = _dbContext.Videos.AsQueryable();

            if (idStatus.HasValue)
            {
                query = query.Where(c => c.Status == idStatus.Value);
            }

            return await query.ToListAsync();
        }

        

     
    }
}

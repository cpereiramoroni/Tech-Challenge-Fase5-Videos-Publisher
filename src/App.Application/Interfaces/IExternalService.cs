using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IExternalService
    {
        Task SaveFileS3(PostVideo input, int id);
        Task PublishedRabbit(int id);
        Task<string> GetZipS3(int id);
    }
}

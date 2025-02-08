using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using App.Domain.Interfaces;
using App.Domain.Models;
using System.Threading.Tasks;

namespace Application.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _repository;
        private readonly IExternalService _externalService;

        public VideosService(IVideosRepository repository, IExternalService externalService)
        {
            _repository = repository;
            _externalService = externalService;
        }

        public async Task<int> Post(PostVideo input)
        {
            // 1º  Grava oi Video no Banco de dados 
            var itemBD = new VideoBD(input.Nome);
            await _repository.PostVideo(itemBD);
            // 2º Upload video to AWS S3
            await _externalService.SaveFileS3(input, itemBD.Id);
            // 3º Insere o id do video em uma fila no RabbitMQ
            await _externalService.PublishedRabbit(itemBD.Id);
            return itemBD.Id;
        }
        public async Task<Video> GetById(int idVideo)
        {

            var itemdb = await _repository.GetById(idVideo);
            var itemVm = new Video(itemdb);
            if (itemdb.Status==2)
                itemVm.Base64Zip = await _externalService.GetZipS3(itemdb.Id);

            return itemVm;
        }



    }
}
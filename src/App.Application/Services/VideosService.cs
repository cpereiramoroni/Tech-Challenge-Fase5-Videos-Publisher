using Amazon.S3;
using Amazon.S3.Model;
using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Domain.Models.External;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _repository;
        private readonly RabbitMQConfig _rabbitConfig;
        private readonly AwsConfig _awsconfig;

        public VideosService(IVideosRepository repository, RabbitMQConfig rabbitConfig, AwsConfig awsconfig)
        {
            _repository = repository;
            _rabbitConfig = rabbitConfig;
            _awsconfig = awsconfig;
        }

        public async Task<int> PostProduto(PostVideo input)
        {
            // 1º  Grava oi Video no Banco de dados 
            var itemBD = new VideoBD(input.Nome);
            await _repository.PostVideo(itemBD);

            // 2º Upload video to AWS S3
            var s3Client = new AmazonS3Client(
                _awsconfig.S3Access,
                _awsconfig.S3Secret,
                Amazon.RegionEndpoint.USEast1);
            
            var putRequest = new PutObjectRequest
            {
                BucketName = "fiapvideo",
                Key = $"{itemBD.Id}.mp4",
                InputStream = new MemoryStream(Convert.FromBase64String(input.Base64)),
                ContentType = "video/mp4"
            };

            await s3Client.PutObjectAsync(putRequest);

            // 3º Insere o id do video em uma fila no RabbitMQ
            var factory = new ConnectionFactory { Uri = _rabbitConfig.Uri };
            using var connection = await factory.CreateConnectionAsync();
            
            using var channel = await connection.CreateChannelAsync();
            
            await channel.ExchangeDeclareAsync(exchange: "fiapVideoEx", "direct", true, false);
            await channel.QueueBindAsync("fiap_video_queue", "fiapVideoEx", "Videos");

            //await channel.QueueDeclareAsync(queue: "fiap_video_queue", durable: true, exclusive: false, autoDelete: false);

            

            string message = itemBD.Id.ToString();
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "fiapVideoEx", routingKey: "Videos", body: body);

            return itemBD.Id;
        }


        public async Task<IList<Video>> GetProdutoByStatus(int? Status)
        {

            var lstBD = await _repository.GetVideosByIdStatus(Status);
            if (lstBD is null)
                return default;

            return lstBD.Select(s => new Video(s)).ToList();
        }



    }
}
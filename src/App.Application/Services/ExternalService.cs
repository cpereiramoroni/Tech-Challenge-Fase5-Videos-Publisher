using Amazon.S3;
using Amazon.S3.Model;
using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Domain.Models.External;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExternalService : IExternalService
    {
        private readonly RabbitMQConfig _rabbitConfig;
        private readonly AwsConfig _awsconfig;

        public ExternalService(RabbitMQConfig rabbitConfig, AwsConfig awsconfig)
        {
            _rabbitConfig = rabbitConfig;
            _awsconfig = awsconfig;
        }

        public async Task SaveFileS3(PostVideo input, int id)
        {

            // 2º Upload video to AWS S3
            var s3Client = new AmazonS3Client(
                _awsconfig.S3Access,
                _awsconfig.S3Secret,
                Amazon.RegionEndpoint.USEast1);

            var putRequest = new PutObjectRequest
            {
                BucketName = "fiapvideo",
                Key = $"{id}.mp4",
                InputStream = new MemoryStream(Convert.FromBase64String(input.Base64)),
                ContentType = "video/mp4"
            };

            await s3Client.PutObjectAsync(putRequest);

        }

        public async Task PublishedRabbit(int id)
        {

            var factory = new ConnectionFactory { Uri = _rabbitConfig.Uri };
            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "fiapVideoEx", "direct", true, false);
            await channel.QueueBindAsync("fiap_video_queue", "fiapVideoEx", "Videos");

            string message = id.ToString();
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "fiapVideoEx", routingKey: "Videos", body: body);
        }

        public async Task<string> GetZipS3(int id)
        {


            var getRequest = new GetObjectRequest
            {
                BucketName = "fiapvideo",
                Key = $"{id}.zip"
            };

            var s3Client = new AmazonS3Client(
                        _awsconfig.S3Access,
                        _awsconfig.S3Secret,
                        Amazon.RegionEndpoint.USEast1);



            using var response = await s3Client.GetObjectAsync(getRequest);
            using var responseStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(responseStream);
            responseStream.Position = 0; // Reset stream position to the beginning

            var base64String = Convert.ToBase64String(responseStream.ToArray());
            return base64String;
        }


    }
}
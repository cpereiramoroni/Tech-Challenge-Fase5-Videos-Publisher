using System;

namespace App.Application.ViewModels.Response
{
    public class Video
    {
        public Video(Domain.Models.VideoBD _video)
        {
            Id = _video.Id;
            Nome = _video.Nome;
            Status = _video.Status;
            DataCadastro = _video.DataCadastro;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Base64Zip { get; set; }
    }
}

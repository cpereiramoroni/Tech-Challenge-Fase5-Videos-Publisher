using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("Videos/")]
    public class VideosController : ControllerBase
    {

        private readonly IVideosService _VideosService;

        public VideosController(IVideosService VideosService)
        {
            _VideosService = VideosService;
        }

        #region [GET/Videos]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(Video))]
        [SwaggerResponse(204, "Requisição concluída, porém não há dados de retorno!")]
        [SwaggerResponse(400, "Condição prévia dada em um ou mais dos campos avaliado como falsa.")]
        [HttpGet("/{idvideo}")]
        [SwaggerOperation(
            Summary = "Busca um Video.",
            Description = @"Endpoint para retornar os Videos que foram cadastrados no sistema. A busca pode ser feita pelos filtros abaixo:</br></br>
                            <b>Parâmetros de entrada:</b></br></br>
                             &bull; <b>idVideo</b>: idVideo. &rArr; <font color='Red'><b>Obrigatório</b></font><br>
            ",
            Tags = new[] { "Videos" }
        )]
        [Consumes("application/json")]
        public async Task<IActionResult> GetVideos([FromRoute] int idvideo)
        {

            var itemrnt = await _VideosService.GetById(idvideo);
            if (itemrnt is null)
                return NoContent();
            return Ok(itemrnt);
        }
        #endregion

        #region POST/Videos
        [SwaggerResponse(201, "A solicitação foi atendida e resultou na criação de um ou mais novos recursos.")]
        [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!")]
        [SwaggerResponse(401, "Requisição requer autenticação do usuário!")]
        [SwaggerResponse(403, "Privilégios insuficientes!")]
        [SwaggerResponse(404, "O recurso solicitado não existe!")]
        [SwaggerResponse(400, "Condição prévia dada em um ou mais dos campos avaliado como falsa!")]
        [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!")]
        [HttpPost("")]
        [SwaggerOperation(
       Summary = "Endpoint para criação de um novo Video.",
       Description = @"Endpoint para criar um novo Video.</br></br>
                            <b>Parâmetros de entrada:</b></br></br>
                              &bull; <b>nome</b>:  Nome do video. &rArr; <font color='red'><b>Obrigatório</b></font><br>
                              &bull; <b>Ativo</b>: True ou False. &rArr; <font color='red'><b>Obrigatório</b></font><br>
                              &bull; <b>base64</b>: base64 do video. &rArr; <font color='red'><b>Obrigatório</b></font><br>
    ", Tags = new[] { "Videos" }
   )]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] PostVideo input)
        {
            var rnt = await _VideosService.Post(input);
            return StatusCode((int)HttpStatusCode.Created,new { id = rnt });

        }
        #endregion

       

    }
}
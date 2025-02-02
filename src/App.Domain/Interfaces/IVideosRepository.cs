﻿using App.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IVideosRepository
    {
        Task PostVideo(VideoBD Video);
        Task<IList<VideoBD>> GetVideosByIdStatus(int? idStatus);
    }
}

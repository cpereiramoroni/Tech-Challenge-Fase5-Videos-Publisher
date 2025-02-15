﻿using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IVideosService
    {
        Task<int> Post(PostVideo input);
        Task<Video> GetById(int idVideo);


    }
}

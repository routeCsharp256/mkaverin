﻿using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    public class MerchandiseController : ControllerBase
    {
        public MerchandiseController()
        {

        }

        [HttpPost("RequestMerch")]
        public async Task<RequestMerchResponseDto> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [HttpGet("InfoMerch")]
        public async Task<GetInfoMerchResponseDto> GetInfoMerch([FromQuery] GetInfoMerchRequestDto requestDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
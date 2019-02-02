using System;
using System.Collections.Generic;
using AutoMapper;
using KeywordStatsApi.Models;
using KeywordStatsApi.Services.Interface;
using KeywordStatsApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KeywordStatsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private IStatsService _statsService;
        private IMapper _mapper;

        public StatsController(IStatsService statsService, IMapper mapper)
        {
            _statsService = statsService;
            _mapper = mapper;
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult<PageStatsVM> Get(string url)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            try
            {
                var stats = _statsService.GetPageStats(url);
                return new PageStatsVM
                {
                    PageUrl = url,
                    KeywordStats = _mapper.Map<IEnumerable<KeywordStat>, IEnumerable<KeywordStatVM>>(stats)
                };
            }
            catch (Exception ex)
            {
                return new PageStatsVM { ErrorDesc = ex.Message };
            }
        }
    }
}
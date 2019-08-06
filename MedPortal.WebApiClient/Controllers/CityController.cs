﻿using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using MedPortal.WebApiClient.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.WebApiClient.Controllers
{
    public class CityController : ControllerBase
    {
        private IRepository<HCity> _cityRepository;

        private readonly IMapper _mapper;

        public CityController(IRepository<HCity> cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        [HttpGet("/api/cities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityRepository.GetAsync();
            var result = _mapper.Map<List<CitySearchModel>>(cities);
            return Ok(result);
        }

        [HttpGet("/api/city")]
        public async Task<IActionResult> GetCitiy(double latitude, double longitude)
        {
            const double fluctuation = 0.01;
            var city = await _cityRepository.FindAsync(c => 
                c.Latitude >= latitude - fluctuation &&
                c.Latitude <= latitude + fluctuation &&
                c.Longitude >= longitude - fluctuation &&
                c.Longitude <= longitude + fluctuation);
            if(city != null)
            {
                var result = _mapper.Map<CitySearchModel>(city);
                return Ok(result);
            }
            else
            {
                return Ok(new CitySearchModel
                {
                    Alias = "noCity",
                    Id = 0
                });
            }
           
        }
    }
}

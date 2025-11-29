using DublinBikesApi.Models;
using DublinBikesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DublinBikesApi.Controllers
{
    [ApiController]
    [Route("api/v2/stations")]
    public class StationsControllerV2 : ControllerBase
    {
        private readonly IStationService _service;

        public StationsControllerV2(IStationService service)
        {
            _service = service;
        }

        // GET com filtros, pesquisa, ordenação e paginação
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] string status = null,
            [FromQuery] int? minBikes = null,
            [FromQuery] string q = null,
            [FromQuery] string sort = "name",
            [FromQuery] string dir = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var stations = _service.GetAll().AsQueryable();

            // Filtro por status
            if (!string.IsNullOrEmpty(status))
            {
                stations = stations.Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            // Filtro por minBikes
            if (minBikes.HasValue)
            {
                stations = stations.Where(s => s.Available_Bikes >= minBikes.Value);
            }

            // Pesquisa por nome ou endereço
            if (!string.IsNullOrEmpty(q))
            {
                stations = stations.Where(s =>
                    s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    s.Address.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            // Ordenação
            stations = (sort.ToLower(), dir.ToLower()) switch
            {
                ("name", "asc") => stations.OrderBy(s => s.Name),
                ("name", "desc") => stations.OrderByDescending(s => s.Name),
                ("availablebikes", "asc") => stations.OrderBy(s => s.Available_Bikes),
                ("availablebikes", "desc") => stations.OrderByDescending(s => s.Available_Bikes),
                ("occupancy", "asc") => stations.OrderBy(s => s.Occupancy),
                ("occupancy", "desc") => stations.OrderByDescending(s => s.Occupancy),
                _ => stations.OrderBy(s => s.Name)
            };

            // Paginação
            var totalItems = stations.Count();
            stations = stations.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                page,
                pageSize,
                totalItems,
                data = stations.ToList()
            };

            return Ok(result);
        }

        // GET por número da estação
        [HttpGet("{number}")]
        public IActionResult GetByNumber(int number)
        {
            var station = _service.GetByNumber(number);
            if (station == null) return NotFound();
            return Ok(station);
        }

        // GET summary / agregados
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _service.GetSummaryAsync();
            return Ok(summary);
        }

        // POST nova estação
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Station station)
        {
            if (station == null) return BadRequest();
            var result = await _service.CreateAsync(station);
            return CreatedAtAction(nameof(GetByNumber), new { number = result.Number }, result);
        }

        // PUT atualizar estação existente
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Station station)
        {
            if (station == null) return BadRequest();
            var result = await _service.UpdateAsync(station);
            return Ok(result);
        }
    }
}

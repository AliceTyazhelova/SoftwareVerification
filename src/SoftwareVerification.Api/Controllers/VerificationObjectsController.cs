using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareVerification.Api.Dto;
using SoftwareVerification.Api.Dtos;
using SoftwareVerification.Domain.Entities;
using SoftwareVerification.Domain.Enums;
using SoftwareVerification.Infrastructure.Data;

namespace SoftwareVerification.Api.Controllers;

[ApiController]
[Route("api/verification-objects")]
public class VerificationObjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public VerificationObjectsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/verification-objects?name=abc&type=Software&result=InProgress
    [HttpGet]
    public async Task<ActionResult<List<VerificationObjectDto>>> GetAll(
        [FromQuery] string? name,
        [FromQuery] VerificationType? type,
        [FromQuery] VerificationResult? result)
    {
        var query = _db.VerificationObjects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var pattern = $"%{name.Trim()}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }

        if (type.HasValue)
            query = query.Where(x => x.Type == type.Value);

        if (result.HasValue)
            query = query.Where(x => x.Result == result.Value);

        var items = await query
            .OrderByDescending(x => x.ReceivedAt)
            .ToListAsync();

        return Ok(items.Select(x => x.ToDto()).ToList());
    }

    // GET /api/verification-objects/{id}
    [HttpGet("{id:int}")] //проверка на целое число, если передадут строку, то 404
    public async Task<ActionResult<VerificationObjectDto>> GetById(int id)
    {
        var entity = await _db.VerificationObjects.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return NotFound();

        return Ok(entity.ToDto());
    }

    // POST /api/verification-objects
    [HttpPost]
    public async Task<ActionResult<VerificationObjectDto>> Create([FromBody] CreateVerificationObjectDto dto)
    {
        var entity = new VerificationObject(
            dto.Name,
            dto.Version,
            dto.Type,
            dto.ReceivedAt);

        _db.VerificationObjects.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(entity.ToDto());
    }

    // PATCH /api/verification-objects/{id}
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<VerificationObjectDto>> Update(int id, [FromBody] UpdateVerificationObjectDto dto)
    {
        var entity = await _db.VerificationObjects.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return NotFound();

        entity.SetResult(dto.Result);
        entity.SetExpertComment(dto.ExpertComment);

        await _db.SaveChangesAsync();

        return Ok(entity.ToDto());
    }
}
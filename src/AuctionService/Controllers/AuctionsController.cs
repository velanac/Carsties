using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auciton = await _context
            .Auctions
            .Include(e => e.Item)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (auciton == null) return NotFound();

        return _mapper.Map<AuctionDto>(auciton);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auciton = _mapper.Map<Auction>(auctionDto);
        // TODO: add current user as seller

        _context.Auctions.Add(auciton);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById), new { auciton.Id }, _mapper.Map<AuctionDto>(auciton));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auciton = await _context
            .Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auciton == null) return NotFound();

        auciton.Item.Make = updateAuctionDto.Make ?? auciton.Item.Make;
        auciton.Item.Model = updateAuctionDto.Model ?? auciton.Item.Model;
        auciton.Item.Color = updateAuctionDto.Color ?? auciton.Item.Color;
        auciton.Item.Mileage = updateAuctionDto.Mileage ?? auciton.Item.Mileage;
        auciton.Item.Year = updateAuctionDto.Year ?? auciton.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Problem saving auction");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FirstOrDefaultAsync(e => e.Id == id);

        if (auction == null) return NotFound();

        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.DTO;
using PTFGym.Extensions;
using PTFGym.Models;

[ApiController]
[Route("api/[controller]")]
public class MembershipController : Controller
{
    private readonly ApplicationDbContext _context;

    public MembershipController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("current/{clanId}")]
    public async Task<ActionResult<ClanarinaDto>> GetCurrentMembership(int clanId)
    {
        var currentMembership = await _context.Clanarina
            .FirstOrDefaultAsync(c => c.ClanId == clanId && c.DatumZavrsetka >= DateTime.Now);

        if (currentMembership == null)
            return NotFound();

        return currentMembership.ToDto();
    }

    [HttpPost("renew")]
    public async Task<ActionResult<ClanarinaDto>> RenewMembership([FromBody] RenewMembershipRequest request)
    {
        var existingMembership = await _context.Clanarina
            .FirstOrDefaultAsync(c => c.ClanId == request.ClanId && c.DatumZavrsetka >= DateTime.Now);

        if (existingMembership == null)
            return NotFound();

        if (!existingMembership.CanRenew())
            return BadRequest("Membership can't be renewed yet.");

        var newMembership = new Clanarina
        {
            ClanId = request.ClanId,
            DatumPocetka = existingMembership.DatumZavrsetka,
            DatumZavrsetka = existingMembership.DatumZavrsetka.AddMonths(1),
            Iznos = existingMembership.Iznos
        };

        _context.Clanarina.Add(newMembership);
        await _context.SaveChangesAsync();

        return newMembership.ToDto();
    }
}

public class RenewMembershipRequest
{
    public int ClanId { get; set; }
}
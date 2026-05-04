using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Queries;

public class AdminQueries(CoreFitnessDbContext context) : IAdminQueries
{
    public async Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync(CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return await context.Users
            .AsNoTracking()
            .Select(user => new AdminUserDTO
            {
                Id = user.Id.Value,
                Email = user.Email.Value,
                FirstName = user.UserName.FirstName,
                LastName = user.UserName.LastName,

                Membership = context.Memberships
                    .Where(m => m.UserId == user.Id)
                    .Select(m => new MembershipDTO
                    {
                       Id = m.Id.Value,
                       StartDate = m.StartDate,
                       EndDate = m.EndDate,

                       MembershipTypeName = context.MembershipTypes
                            .Where(t => t.Id == m.TypeId)
                            .Select(t => t.Name.Value)
                            .FirstOrDefault() ?? "Unknown",

                            IsActive = !m.IsManuallyDeactivated && m.EndDate >= today
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(ct);
    }
}

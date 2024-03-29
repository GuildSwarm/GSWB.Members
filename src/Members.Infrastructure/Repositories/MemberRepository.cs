﻿using Members.Application;
using Members.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TGF.CA.Infrastructure.DB.Repository;
using TGF.Common.ROP.HttpResult;

namespace Members.Infrastructure.Repositories
{
    public class MemberRepository(MembersDbContext aContext, ILogger<MemberRepository> aLogger)
        : RepositoryBase<MemberRepository, MembersDbContext>(aContext, aLogger), IMemberRepository, ISortRepository
    {
        public async Task<IHttpResult<IEnumerable<Member>>> GetMembersListAsync(
            int aPage, int aPageSize,
            string aSortBy,
            string? aDiscordNameFilter = null, string? aGameHandleFilter = null, ulong? aRoleIdFilter = null, bool? aIsVerifiedFilter = null,
            CancellationToken aCancellationToken = default)
        => await TryQueryAsync(async (aCancellationToken) =>
        {
            var lQuery = _context.Members
            .Include(m => m.Roles.OrderByDescending(role => role.Position))
            .AsQueryable();

            lQuery = ApplyFilters(lQuery, aDiscordNameFilter, aGameHandleFilter, aRoleIdFilter, aIsVerifiedFilter);
            lQuery = ApplySorting(lQuery, aSortBy);

            return await lQuery
                .Skip((aPage - 1) * aPageSize)
                .Take(aPageSize)
                .ToListAsync(aCancellationToken) as IEnumerable<Member>;
        }, aCancellationToken);

        public async Task<IHttpResult<Member>> GetByDiscordUserIdAsync(ulong aDiscordUserId, CancellationToken aCancellationToken = default)
        => await TryQueryAsync(async (aCancellationToken) =>
        {
            return await _context.Members
            .Include(m => m.Roles.OrderByDescending(r => r.Position))
            .SingleOrDefaultAsync(m => m.DiscordUserId == aDiscordUserId, aCancellationToken);
        }, aCancellationToken)
        .Verify(member => member != null, InfrastructureErrors.MembersDb.NotFoundDiscordUserId)
        .Map(member => member!);

        public async Task<IHttpResult<Member>> Add(Member aNewMember, CancellationToken aCancellationToken = default)
            => await TryCommandAsync(() => _context.Members.Add(aNewMember).Entity, aCancellationToken);

        public async Task<IHttpResult<Member>> Update(Member aMember, CancellationToken aCancellationToken = default)
            => await TryCommandAsync(() => _context.Members.Update(aMember).Entity, aCancellationToken);

        public async Task<IHttpResult<Member>> Delete(Member aMemberToDelete, CancellationToken aCancellationToken = default)
            => await TryCommandAsync(() => _context.Members.Remove(aMemberToDelete).Entity, aCancellationToken);

        public async Task<IHttpResult<int>> GetCountAsync(CancellationToken aCancellationToken = default)
        => await TryQueryAsync(async (aCancellationToken)
            => await _context.Members.CountAsync(aCancellationToken)
        , aCancellationToken);

        #region Private

        private static IQueryable<Member> ApplyFilters(
            IQueryable<Member> aQuery,
            string? aDiscordNameFilter,
            string? aGameHandleFilter,
            ulong? aRoleIdFilter,
            bool? aIsVerifiedFilter)
        {
            if (!string.IsNullOrEmpty(aDiscordNameFilter))
            {
                var lLowercaseDiscordNameFilter = $"%{aDiscordNameFilter.ToLowerInvariant()}%";
                aQuery = aQuery.Where(m => EF.Functions.Like(m.DiscordGuildDisplayName.ToLower(), lLowercaseDiscordNameFilter));
            }

            if (!string.IsNullOrEmpty(aGameHandleFilter))
            {
                var lLowercaseGameHandleFilter = $"%{aGameHandleFilter.ToLowerInvariant()}%";
                aQuery = aQuery.Where(m => !string.IsNullOrWhiteSpace(m.GameHandle) && EF.Functions.Like(m.GameHandle.ToLower(), lLowercaseGameHandleFilter));
            }

            if (aRoleIdFilter.HasValue)
                aQuery = aQuery.Where(m => m.Roles.Any(r => r.DiscordRoleId == aRoleIdFilter.Value));

            if (aIsVerifiedFilter.HasValue)
                aQuery = aQuery.Where(m => m.IsGameHandleVerified == aIsVerifiedFilter.Value);

            return aQuery;
        }

        private static IQueryable<Member> ApplySorting(IQueryable<Member> aQuery, string aSortBy)
        => aSortBy == nameof(Member.Roles)
            ? aQuery.OrderBy(m => m.Roles
                .Max(rc => rc.Position))
            : ISortRepository.ApplySorting(aQuery, aSortBy);

        #endregion

    }

}

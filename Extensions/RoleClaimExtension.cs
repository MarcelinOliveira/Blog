using System.Security.Claims;
using BlogEF.Data;
using BlogVisualStudio.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogVisualStudio.Extensions;

public static class RoleClaimExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>()
        {
            new(ClaimTypes.Email, user.Email),
        };
        result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, user.Slug)));
        return result;
    }
}
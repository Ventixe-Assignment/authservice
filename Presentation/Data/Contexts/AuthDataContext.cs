using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Data.Contexts;

public class AuthDataContext(DbContextOptions<AuthDataContext> options) : IdentityDbContext<IdentityUser>(options)
{
}

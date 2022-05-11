using Grad_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project.Data
{
    public class ApplicationUser : IdentityUser
    {
        
        public string? Address { get; set; }
        public decimal? totalAmount { get; set; }
        
    }
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}
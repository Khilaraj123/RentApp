using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentApp.Domain.Entities.Users;
using System;

namespace RentApp.Persistence.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationUser Relationships that throw warnings due to multiple relationships
        builder.Entity<ApplicationUser>(b =>
        {
            b.HasMany(e => e.CustomerBookings).WithOne().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.Restrict);
            b.HasMany(e => e.OwnerBookings).WithOne().HasForeignKey("OwnerId").OnDelete(DeleteBehavior.Restrict);
            
            b.HasMany(e => e.ReviewsWritten).WithOne().HasForeignKey("ReviewerId").OnDelete(DeleteBehavior.Restrict);
            b.HasMany(e => e.ReviewsReceived).WithOne().HasForeignKey("RevieweeId").OnDelete(DeleteBehavior.Restrict);
            
            b.HasMany(e => e.ConversationsAsCustomer).WithOne().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.Restrict);
            b.HasMany(e => e.ConversationsAsOwner).WithOne().HasForeignKey("OwnerId").OnDelete(DeleteBehavior.Restrict);
        });
    }
}

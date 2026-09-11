using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentApp.Domain.Entities.Users;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Entities.Reviews;
using RentApp.Domain.Entities.Categories;
using RentApp.Domain.Entities.Wishlists;
using System;

namespace RentApp.Persistence.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<BookingItem> BookingItems { get; set; }
    public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }
    public DbSet<AvailabilityRule> AvailabilityRules { get; set; }
    public DbSet<ListingImage> ListingImages { get; set; }
    public DbSet<ListingPolicy> ListingPolicies { get; set; }
    public DbSet<PricingRule> PricingRules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationUser Relationships
        builder.Entity<ApplicationUser>(b =>
        {
            b.HasMany(e => e.CustomerBookings)
                .WithOne()
                .HasForeignKey(b => b.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.OwnerBookings)
                .WithOne()
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            b.HasMany(e => e.ReviewsWritten)
                .WithOne()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.ReviewsReceived)
                .WithOne()
                .HasForeignKey(r => r.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(e => e.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Category self-referencing relationship
        builder.Entity<Category>(b =>
        {
            b.HasOne(c => c.Parent)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

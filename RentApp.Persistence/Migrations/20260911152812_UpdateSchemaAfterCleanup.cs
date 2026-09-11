using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaAfterCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityRule_Listing_ListingId",
                table: "AvailabilityRule");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_CustomerId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_OwnerId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingItem_Booking_BookingId",
                table: "BookingItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStatusHistory_Booking_BookingId",
                table: "BookingStatusHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_AspNetUsers_OwnerId",
                table: "Listing");

            migrationBuilder.DropForeignKey(
                name: "FK_Listing_Category_CategoryId",
                table: "Listing");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingImage_Listing_ListingId",
                table: "ListingImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingPolicy_Listing_ListingId",
                table: "ListingPolicy");

            migrationBuilder.DropForeignKey(
                name: "FK_PricingRule_Listing_ListingId",
                table: "PricingRule");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_RevieweeId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_ReviewerId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImage_Review_ReviewId",
                table: "ReviewImage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_AspNetUsers_UserId",
                table: "UserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_AspNetUsers_ApplicationUserId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItem_Wishlist_WishlistId",
                table: "WishlistItem");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Evidence");

            migrationBuilder.DropTable(
                name: "MessageAttachment");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Refund");

            migrationBuilder.DropTable(
                name: "RentalAgreement");

            migrationBuilder.DropTable(
                name: "ReportEvidence");

            migrationBuilder.DropTable(
                name: "Dispute");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItem",
                table: "WishlistItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricingRule",
                table: "PricingRule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingPolicy",
                table: "ListingPolicy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingImage",
                table: "ListingImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Listing",
                table: "Listing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStatusHistory",
                table: "BookingStatusHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingItem",
                table: "BookingItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityRule",
                table: "AvailabilityRule");

            migrationBuilder.DropColumn(
                name: "EmailNotificationsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PushNotificationsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SmsNotificationsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "WishlistItem",
                newName: "WishlistItems");

            migrationBuilder.RenameTable(
                name: "Wishlist",
                newName: "Wishlists");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "PricingRule",
                newName: "PricingRules");

            migrationBuilder.RenameTable(
                name: "ListingPolicy",
                newName: "ListingPolicies");

            migrationBuilder.RenameTable(
                name: "ListingImage",
                newName: "ListingImages");

            migrationBuilder.RenameTable(
                name: "Listing",
                newName: "Listings");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "BookingStatusHistory",
                newName: "BookingStatusHistories");

            migrationBuilder.RenameTable(
                name: "BookingItem",
                newName: "BookingItems");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "AvailabilityRule",
                newName: "AvailabilityRules");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItem_WishlistId",
                table: "WishlistItems",
                newName: "IX_WishlistItems_WishlistId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_ApplicationUserId",
                table: "Wishlists",
                newName: "IX_Wishlists_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ReviewerId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_RevieweeId",
                table: "Reviews",
                newName: "IX_Reviews_RevieweeId");

            migrationBuilder.RenameIndex(
                name: "IX_PricingRule_ListingId",
                table: "PricingRules",
                newName: "IX_PricingRules_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingPolicy_ListingId",
                table: "ListingPolicies",
                newName: "IX_ListingPolicies_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingImage_ListingId",
                table: "ListingImages",
                newName: "IX_ListingImages_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_OwnerId",
                table: "Listings",
                newName: "IX_Listings_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Listing_CategoryId",
                table: "Listings",
                newName: "IX_Listings_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ParentId",
                table: "Categories",
                newName: "IX_Categories_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingStatusHistory_BookingId",
                table: "BookingStatusHistories",
                newName: "IX_BookingStatusHistories_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingItem_BookingId",
                table: "BookingItems",
                newName: "IX_BookingItems_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_OwnerId",
                table: "Bookings",
                newName: "IX_Bookings_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityRule_ListingId",
                table: "AvailabilityRules",
                newName: "IX_AvailabilityRules_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricingRules",
                table: "PricingRules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingPolicies",
                table: "ListingPolicies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingImages",
                table: "ListingImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listings",
                table: "Listings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStatusHistories",
                table: "BookingStatusHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityRules",
                table: "AvailabilityRules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    TokenFamilyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RenterId",
                table: "Bookings",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityRules_Listings_ListingId",
                table: "AvailabilityRules",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItems_Bookings_BookingId",
                table: "BookingItems",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_OwnerId",
                table: "Bookings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_RenterId",
                table: "Bookings",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStatusHistories_Bookings_BookingId",
                table: "BookingStatusHistories",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingImages_Listings_ListingId",
                table: "ListingImages",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingPolicies_Listings_ListingId",
                table: "ListingPolicies",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_OwnerId",
                table: "Listings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PricingRules_Listings_ListingId",
                table: "PricingRules",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewImage_Reviews_ReviewId",
                table: "ReviewImage",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_RevieweeId",
                table: "Reviews",
                column: "RevieweeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Wishlists_WishlistId",
                table: "WishlistItems",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_AspNetUsers_ApplicationUserId",
                table: "Wishlists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityRules_Listings_ListingId",
                table: "AvailabilityRules");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingItems_Bookings_BookingId",
                table: "BookingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_OwnerId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_RenterId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStatusHistories_Bookings_BookingId",
                table: "BookingStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingImages_Listings_ListingId",
                table: "ListingImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingPolicies_Listings_ListingId",
                table: "ListingPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_OwnerId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_PricingRules_Listings_ListingId",
                table: "PricingRules");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewImage_Reviews_ReviewId",
                table: "ReviewImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_RevieweeId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Wishlists_WishlistId",
                table: "WishlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_AspNetUsers_ApplicationUserId",
                table: "Wishlists");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricingRules",
                table: "PricingRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Listings",
                table: "Listings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingPolicies",
                table: "ListingPolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingImages",
                table: "ListingImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStatusHistories",
                table: "BookingStatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RenterId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityRules",
                table: "AvailabilityRules");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "Wishlist");

            migrationBuilder.RenameTable(
                name: "WishlistItems",
                newName: "WishlistItem");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfile");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "PricingRules",
                newName: "PricingRule");

            migrationBuilder.RenameTable(
                name: "Listings",
                newName: "Listing");

            migrationBuilder.RenameTable(
                name: "ListingPolicies",
                newName: "ListingPolicy");

            migrationBuilder.RenameTable(
                name: "ListingImages",
                newName: "ListingImage");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "BookingStatusHistories",
                newName: "BookingStatusHistory");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameTable(
                name: "BookingItems",
                newName: "BookingItem");

            migrationBuilder.RenameTable(
                name: "AvailabilityRules",
                newName: "AvailabilityRule");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_ApplicationUserId",
                table: "Wishlist",
                newName: "IX_Wishlist_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItem",
                newName: "IX_WishlistItem_WishlistId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfile",
                newName: "IX_UserProfile_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Review",
                newName: "IX_Review_ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_RevieweeId",
                table: "Review",
                newName: "IX_Review_RevieweeId");

            migrationBuilder.RenameIndex(
                name: "IX_PricingRules_ListingId",
                table: "PricingRule",
                newName: "IX_PricingRule_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_OwnerId",
                table: "Listing",
                newName: "IX_Listing_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_CategoryId",
                table: "Listing",
                newName: "IX_Listing_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingPolicies_ListingId",
                table: "ListingPolicy",
                newName: "IX_ListingPolicy_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImage",
                newName: "IX_ListingImage_ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentId",
                table: "Category",
                newName: "IX_Category_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingStatusHistories_BookingId",
                table: "BookingStatusHistory",
                newName: "IX_BookingStatusHistory_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_OwnerId",
                table: "Booking",
                newName: "IX_Booking_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingItems_BookingId",
                table: "BookingItem",
                newName: "IX_BookingItem_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityRules_ListingId",
                table: "AvailabilityRule",
                newName: "IX_AvailabilityRule_ListingId");

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotificationsEnabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PushNotificationsEnabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SmsNotificationsEnabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Booking",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItem",
                table: "WishlistItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricingRule",
                table: "PricingRule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Listing",
                table: "Listing",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingPolicy",
                table: "ListingPolicy",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingImage",
                table: "ListingImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStatusHistory",
                table: "BookingStatusHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingItem",
                table: "BookingItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityRule",
                table: "AvailabilityRule",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastMessageAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastMessagePreview = table.Column<string>(type: "text", nullable: true),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerLastReadAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OwnerUnreadCount = table.Column<int>(type: "integer", nullable: false),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    RenterLastReadAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RenterUnreadCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dispute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedAdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClosedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    InternalNotes = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    RefundId = table.Column<Guid>(type: "uuid", nullable: true),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Resolution = table.Column<string>(type: "text", nullable: true),
                    ResolutionType = table.Column<int>(type: "integer", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispute_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dispute_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionUrl = table.Column<string>(type: "text", nullable: true),
                    Channel = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryStatus = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ReadAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceType = table.Column<int>(type: "integer", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TemplateData = table.Column<string>(type: "text", nullable: true),
                    TemplateName = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    Gateway = table.Column<int>(type: "integer", nullable: false),
                    GatewayResponse = table.Column<string>(type: "text", nullable: true),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    PaidAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "text", nullable: true),
                    PaymentReference = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount_Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentalAgreement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgreementNumber = table.Column<string>(type: "text", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CancellationPolicy = table.Column<string>(type: "text", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    CancelledAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DamagePolicy = table.Column<string>(type: "text", nullable: false),
                    DocumentFileName = table.Column<string>(type: "text", nullable: false),
                    DocumentHash = table.Column<string>(type: "text", nullable: false),
                    DocumentSize = table.Column<long>(type: "bigint", nullable: false),
                    DocumentUrl = table.Column<string>(type: "text", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingTitle = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerIpAddress = table.Column<string>(type: "text", nullable: true),
                    OwnerUserAgent = table.Column<string>(type: "text", nullable: true),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    RenterIpAddress = table.Column<string>(type: "text", nullable: true),
                    RenterUserAgent = table.Column<string>(type: "text", nullable: true),
                    SignedByOwnerAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SignedByOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SignedByRenterAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SignedByRenterId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DepositAmount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DepositAmount_Currency = table.Column<string>(type: "text", nullable: false),
                    RentalAmount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RentalAmount_Currency = table.Column<string>(type: "text", nullable: false),
                    RentalPeriod_EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RentalPeriod_StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAgreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalAgreement_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RentalAgreement_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionTaken = table.Column<int>(type: "integer", nullable: true),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedModeratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResolutionNotes = table.Column<string>(type: "text", nullable: true),
                    ReviewedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Evidence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisputeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidence_Dispute_DisputeId",
                        column: x => x.DisputeId,
                        principalTable: "Dispute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageAttachment_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    RefundedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount_Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refund_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Refund_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportEvidence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EvidenceUrl = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportEvidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportEvidence_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_CustomerId",
                table: "Conversation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_OwnerId",
                table: "Conversation",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_ApplicationUserId",
                table: "Dispute",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_BookingId",
                table: "Dispute",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidence_DisputeId",
                table: "Evidence",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ApplicationUserId",
                table: "Message",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachment_MessageId",
                table: "MessageAttachment",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ApplicationUserId",
                table: "Payment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_ApplicationUserId",
                table: "Refund",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_PaymentId",
                table: "Refund",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_ApplicationUserId",
                table: "RentalAgreement",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_BookingId",
                table: "RentalAgreement",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ApplicationUserId",
                table: "Report",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportEvidence_ReportId",
                table: "ReportEvidence",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityRule_Listing_ListingId",
                table: "AvailabilityRule",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_CustomerId",
                table: "Booking",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_OwnerId",
                table: "Booking",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItem_Booking_BookingId",
                table: "BookingItem",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStatusHistory_Booking_BookingId",
                table: "BookingStatusHistory",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentId",
                table: "Category",
                column: "ParentId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_AspNetUsers_OwnerId",
                table: "Listing",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listing_Category_CategoryId",
                table: "Listing",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingImage_Listing_ListingId",
                table: "ListingImage",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingPolicy_Listing_ListingId",
                table: "ListingPolicy",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PricingRule_Listing_ListingId",
                table: "PricingRule",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_RevieweeId",
                table: "Review",
                column: "RevieweeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_ReviewerId",
                table: "Review",
                column: "ReviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewImage_Review_ReviewId",
                table: "ReviewImage",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_AspNetUsers_UserId",
                table: "UserProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_AspNetUsers_ApplicationUserId",
                table: "Wishlist",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItem_Wishlist_WishlistId",
                table: "WishlistItem",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

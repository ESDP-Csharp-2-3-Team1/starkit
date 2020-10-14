using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Starkit.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    IIN = table.Column<string>(nullable: true),
                    CityPhone = table.Column<string>(nullable: true),
                    AvatarPath = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsTermsAccepted = table.Column<bool>(nullable: false),
                    RestaurantId = table.Column<string>(nullable: true),
                    IdOfTheSelectedRestaurateur = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    GoogleMapsApi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSiteCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImgPathCarousel1 = table.Column<string>(nullable: true),
                    ImgPathCarousel2 = table.Column<string>(nullable: true),
                    ImgPathCarousel3 = table.Column<string>(nullable: true),
                    DishNameCarousel1 = table.Column<string>(nullable: true),
                    DishNameCarousel2 = table.Column<string>(nullable: true),
                    DishNameCarousel3 = table.Column<string>(nullable: true),
                    DishTextCarousel1 = table.Column<string>(nullable: true),
                    DishTextCarousel2 = table.Column<string>(nullable: true),
                    DishTextCarousel3 = table.Column<string>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSiteCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostalAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NameRestaurant = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    ContactPerson = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    RestaurantInformation = table.Column<string>(nullable: false),
                    DomainAvailability = table.Column<bool>(nullable: false),
                    DomainName = table.Column<string>(nullable: true),
                    DomainRegistrar = table.Column<string>(nullable: true),
                    LogoPath = table.Column<string>(nullable: true),
                    WorkSchedule = table.Column<string>(nullable: true),
                    TotalNumberSeats = table.Column<int>(nullable: false),
                    AvailableNumberSeats = table.Column<int>(nullable: false),
                    OrderConditions = table.Column<string>(nullable: true),
                    BookingTerms = table.Column<string>(nullable: true),
                    GoogleMapsApi = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditedTime = table.Column<DateTime>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true),
                    EditorId = table.Column<string>(nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    RestaurantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menu_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    OrderTime = table.Column<DateTime>(nullable: false),
                    DeliveryMethod = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    OrderNum = table.Column<int>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    RestaurantId = table.Column<string>(nullable: true),
                    Hide = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Capacity = table.Column<int>(nullable: false),
                    IconUrl = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Location = table.Column<int>(nullable: false),
                    IsSmoking = table.Column<bool>(nullable: false),
                    IsQuiet = table.Column<bool>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    RestaurantId = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    EditorId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EditedDate = table.Column<DateTime>(nullable: false),
                    isDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tables_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tables_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tables_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditedTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategories_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    BookFrom = table.Column<string>(nullable: false),
                    BookTo = table.Column<string>(nullable: false),
                    ClientName = table.Column<string>(nullable: false),
                    Pax = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EditorId = table.Column<string>(nullable: true),
                    EditedDate = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    TableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(nullable: false),
                    SubCategoryId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Calorie = table.Column<double>(nullable: false),
                    ProperNutrition = table.Column<bool>(nullable: false),
                    Vegetarian = table.Column<bool>(nullable: false),
                    Visibility = table.Column<bool>(nullable: false),
                    Ingredients = table.Column<string>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    EditorId = table.Column<string>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dishes_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dishes_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dishes_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dishes_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingTables",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BookingId = table.Column<string>(nullable: true),
                    TableId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingTables_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingTables_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuDishes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MenuId = table.Column<string>(nullable: true),
                    DishId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuDishes_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Validity = table.Column<string>(nullable: false),
                    At = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    EditorId = table.Column<string>(nullable: true),
                    FirstDishId = table.Column<string>(nullable: true),
                    SecondDishId = table.Column<string>(nullable: true),
                    ThirdDishId = table.Column<string>(nullable: true),
                    RestaurantId = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Dishes_FirstDishId",
                        column: x => x.FirstDishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Dishes_SecondDishId",
                        column: x => x.SecondDishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Dishes_ThirdDishId",
                        column: x => x.ThirdDishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdersProducts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DishId = table.Column<string>(nullable: true),
                    MenuId = table.Column<string>(nullable: true),
                    StockId = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatorId",
                table: "Bookings",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EditorId",
                table: "Bookings",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RestaurantId",
                table: "Bookings",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TableId",
                table: "Bookings",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTables_BookingId",
                table: "BookingTables",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTables_TableId",
                table: "BookingTables",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RestaurantId",
                table: "Categories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CreatorId",
                table: "Dishes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_EditorId",
                table: "Dishes",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_SubCategoryId",
                table: "Dishes",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CreatorId",
                table: "Menu",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_EditorId",
                table: "Menu",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_RestaurantId",
                table: "Menu",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_DishId",
                table: "MenuDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuDishes_MenuId",
                table: "MenuDishes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_DishId",
                table: "OrdersProducts",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_MenuId",
                table: "OrdersProducts",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_OrderId",
                table: "OrdersProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_StockId",
                table: "OrdersProducts",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_CreatorId",
                table: "Stocks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_EditorId",
                table: "Stocks",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_FirstDishId",
                table: "Stocks",
                column: "FirstDishId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_RestaurantId",
                table: "Stocks",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_SecondDishId",
                table: "Stocks",
                column: "SecondDishId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ThirdDishId",
                table: "Stocks",
                column: "ThirdDishId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_RestaurantId",
                table: "SubCategories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_UserId",
                table: "SubCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_CreatorId",
                table: "Tables",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_EditorId",
                table: "Tables",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_RestaurantId",
                table: "Tables",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookingTables");

            migrationBuilder.DropTable(
                name: "DataSiteCards");

            migrationBuilder.DropTable(
                name: "LegalAddresses");

            migrationBuilder.DropTable(
                name: "MenuDishes");

            migrationBuilder.DropTable(
                name: "OrdersProducts");

            migrationBuilder.DropTable(
                name: "PostalAddresses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Starkit.Migrations
{
    public partial class AddTableDishes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "96afcf32-3063-4d82-94ce-c98231e310db");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "c089d556-8fbe-4677-b95c-934a4b0f8a39");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "d33ddd7f-e169-451e-bd26-dfd6d48ee97a");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "f5d1c412-8ea1-4068-8ec5-9f727e1b8f32");

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CategoryId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Calorie = table.Column<double>(nullable: false),
                    ProperNutrition = table.Column<bool>(nullable: false),
                    Vegetarian = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "7da006f5-e64c-40f2-ab1e-cf53f9e856b2", "Первые блюда" },
                    { "542a02eb-ee81-463a-9f8a-7c694bae9a6f", "Вторые блюда" },
                    { "0b75941f-eabd-49e3-8648-9540acafb5ba", "Десерты" },
                    { "61639dc3-805e-493f-9c50-3c0171722650", "Напитки" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "0b75941f-eabd-49e3-8648-9540acafb5ba");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "542a02eb-ee81-463a-9f8a-7c694bae9a6f");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "61639dc3-805e-493f-9c50-3c0171722650");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "7da006f5-e64c-40f2-ab1e-cf53f9e856b2");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "96afcf32-3063-4d82-94ce-c98231e310db", "Первые блюда" },
                    { "f5d1c412-8ea1-4068-8ec5-9f727e1b8f32", "Вторые блюда" },
                    { "c089d556-8fbe-4677-b95c-934a4b0f8a39", "Десерты блюда" },
                    { "d33ddd7f-e169-451e-bd26-dfd6d48ee97a", "Напитки" }
                });
        }
    }
}

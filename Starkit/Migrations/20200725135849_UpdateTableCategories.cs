using Microsoft.EntityFrameworkCore.Migrations;

namespace Starkit.Migrations
{
    public partial class UpdateTableCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { "b7074b09-112f-4959-98b2-b49c84b14727", "Первые блюда" },
                    { "b1174780-7a9c-4265-9ca8-b09820d4330e", "Вторые блюда" },
                    { "70144069-feac-477c-9c69-9bf4b520860b", "Десерты" },
                    { "8b1ae087-1cea-4679-95d8-a93647ad93dd", "Напитки" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "70144069-feac-477c-9c69-9bf4b520860b");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "8b1ae087-1cea-4679-95d8-a93647ad93dd");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "b1174780-7a9c-4265-9ca8-b09820d4330e");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "b7074b09-112f-4959-98b2-b49c84b14727");

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
        }
    }
}

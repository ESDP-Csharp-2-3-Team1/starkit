using Microsoft.EntityFrameworkCore.Migrations;

namespace Starkit.Migrations
{
    public partial class AddDataSiteFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingSubtitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingTitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DishesSubtitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DishesTitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPathBooking",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPathDishes",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPathMenu",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgPathSpecialOffers",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuSubtitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuTitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialOffersSubtitle",
                table: "DataSiteCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialOffersTitle",
                table: "DataSiteCards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingSubtitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "BookingTitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "DishesSubtitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "DishesTitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "ImgPathBooking",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "ImgPathDishes",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "ImgPathMenu",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "ImgPathSpecialOffers",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "MenuSubtitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "MenuTitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "SpecialOffersSubtitle",
                table: "DataSiteCards");

            migrationBuilder.DropColumn(
                name: "SpecialOffersTitle",
                table: "DataSiteCards");
        }
    }
}

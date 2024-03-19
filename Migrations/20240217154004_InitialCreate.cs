using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagementSystem.Migrations
{
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
         name: "Products",
         columns: table => new
         {
             ProductID = table.Column<int>(nullable: false)
              .Annotation("SqlServer:Identity", "1, 1"),
             Name = table.Column<string>(nullable: true),
             Price = table.Column<decimal>(type: "decimal(18, 2)"),
             Quantity = table.Column<int>(nullable: false),
             CreationDate = table.Column<DateTime>(nullable: false)
         },
      constraints: table =>
      {
          table.PrimaryKey("PK_Products", x => x.ProductID);
      });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

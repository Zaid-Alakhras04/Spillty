using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Split.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses");

            migrationBuilder.CreateTable(
                name: "ExpenseShares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    OwnedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseShares_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseShares_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseShares_ExpenseId",
                table: "ExpenseShares",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseShares_MemberId",
                table: "ExpenseShares",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseShares");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Members_MemberId",
                table: "Expenses",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

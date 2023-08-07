using Microsoft.EntityFrameworkCore.Migrations;

namespace Discord_Core.Migrations
{
    public class FillData
    {
        public static void Insert(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("User", new string[] { "Id", "Current_Level", "Current_Exp", "DiscordId", "Coins" },
                new object?[,]
                {
                    {1, 2, 319, "261136853845934093", 0 },
                    {2, 1, 10, "676546644451262469", 0 },
                    {3, 1, 2, "761889678123073576", 0 }
                });
        }
    }
}
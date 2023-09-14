using Microsoft.EntityFrameworkCore.Migrations;

namespace Discord_Core.Migrations
{
    public class FillData
    {
        public static void Insert(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("User", new string[] { "Id", "IngameName", "DiscordId" },
                new object?[,]
                {
                    {1, "", "261136853845934093" },
                    {2, "Aqua", "" },
                    {3, "Lucy", "" }
                });

            migrationBuilder.InsertData("Buff", new string[] { "Id", "Name", "Description" },
                new object?[,]
                {
                    {1, "Vice President", "+10% Construction Speed, +10% Research Speed, +10% Training Speed"},
                    {2, "Minister of Interest", "+80% Resource Production Speed, Power to appoint and remove officials in the presidents place"},
                    {3, "Minister of Health", "+100% Healing Speed, +5.000 Infimary Capacity"},
                    {4, "Minisier of Defense", "+10% Troops Lethality"},
                    {5, "Minister of Strategy", "+5% Troop Attack, + 2.500 Troop Deployment Capacity"},
                    {6, "Minister of Education", "+50% Training Speed, +200 Training Capacity"}
                });

            migrationBuilder.InsertData("Timer", new string[]{"Id", "Name", "TimerAmount"},
                new object[,]
                {
                    {1, "Buff Cooldown", 2000}
                });

            migrationBuilder.InsertData("WaitingList", new string[]{"Id", "UserId", "BuffId", "TimerId"},
                new object[,]
                {
                    {1, 1, 1, 1},
                    {2, 2, 2, 1},
                    {3, 3, 2, 1}
                });
        }
    }
}
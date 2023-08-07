using Microsoft.EntityFrameworkCore.Migrations;

namespace Discord_Core.Database.View;

public class CreateViews
{
    public static void Insert(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
                CREATE VIEW InventoryItemView  AS
                SELECT Inv.Id, It.Name AS ItemName, T.Name AS ItemType, Q.Name AS ItemQuality, Inv.Quantity, U.DiscordId
                FROM Inventory AS Inv
                JOIN Item AS It ON Inv.ItemId = It.Id
                JOIN [dbo].[User] AS U ON Inv.UserId = U.Id
                JOIN Item_Type AS T ON It.Type = T.Id
                JOIN Quality AS Q ON It.Quality = Q.Id
            ");
    }
    

}

using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace KDMSModel
{
    public class DbHelper : DbContext
    {
        public DbHelper()
            : base("strConn")
        {
            //自动创建表，如果Entity有改到就更新到表结构
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbHelper, ReportingDbMigrationsConfiguration>());
            Database.SetInitializer<DbHelper>(null);
        }        

        //public DbSet<Cate> Cate { get; set; }

        //public DbSet<MealShop> MealShop { get; set; }

        /// <summary>
        /// 餐厅表
        /// </summary>
        public DbSet<Restaurant> Restaurant { get; set; }

        /// <summary>
        /// 菜单表
        /// </summary>
        public DbSet<RestaurantMenu> RestaurantMenu { get; set; }

    }

    internal sealed class ReportingDbMigrationsConfiguration : DbMigrationsConfiguration<DbHelper>
    {
        public ReportingDbMigrationsConfiguration()
        {
            //AutomaticMigrationsEnabled = true;//任何Model Class的修改將會直接更新DB
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
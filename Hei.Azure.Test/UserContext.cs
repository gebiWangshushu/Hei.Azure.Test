using Hei.Azure.Test.Models;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure;

namespace Hei.Azure.Test
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        ///// <summary>
        ///// 重写连接数据库
        ///// </summary>
        ///// <param name="optionsBuilder"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // 定义要使用的数据库
        //    var connectionString = PassportConfig.Get("CosmosDb:ConnectionString");
        //    var databaseName = PassportConfig.Get("CosmosDb:DatabaseName");
        //    optionsBuilder.UseCosmos(connectionString, databaseName);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //配置分区键
            modelBuilder.Entity<UserModel>()
                .HasPartitionKey(o => o.PartitionKey)
                .HasKey(o => o.PartitionKey);

            //调用EnsureCreated方法只针对与添加数据有效，但是数据库如果有数据的话，
            //也就是对数据更改将无效

            #region 数据库数据映射

            modelBuilder.Entity<UserModel>().HasData(
                   new UserModel { PartitionKey = "1", Id = 1, Name = "张无忌", Age = 12, Address = "北京市西城区鲍家街43号", Remark = "中央音乐学院" },
                   new UserModel { PartitionKey = "2", Id = 2, Name = "令狐冲", Age = 20, Address = "佛山市南海区灯湖东路6号", Remark = "广发商学院" });

            #endregion 数据库数据映射
        }
    }
}
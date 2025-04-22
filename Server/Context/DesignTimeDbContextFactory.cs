﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ShopPlatform.Server.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BlazorShopDbContext>
    {
        public BlazorShopDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlazorShopDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=BlazorShopDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BlazorShopDbContext(optionsBuilder.Options);
        }
    }
}

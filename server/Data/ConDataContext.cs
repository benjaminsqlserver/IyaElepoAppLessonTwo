using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using IyaElepoApp.Models.ConData;

namespace IyaElepoApp.Data
{
  public partial class ConDataContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public ConDataContext(DbContextOptions<ConDataContext> options):base(options)
    {
    }

    public ConDataContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IyaElepoApp.Models.ConData.Customer>()
              .HasOne(i => i.CustomerType)
              .WithMany(i => i.Customers)
              .HasForeignKey(i => i.CustomerTypeID)
              .HasPrincipalKey(i => i.CustomerTypeID);
        builder.Entity<IyaElepoApp.Models.ConData.Product>()
              .HasOne(i => i.ProductType)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.ProductTypeID)
              .HasPrincipalKey(i => i.ProductTypeID);
        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .HasOne(i => i.Customer)
              .WithMany(i => i.ProductSales)
              .HasForeignKey(i => i.CustomerID)
              .HasPrincipalKey(i => i.CustomerID);
        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .HasOne(i => i.Product)
              .WithMany(i => i.ProductSales)
              .HasForeignKey(i => i.ProductID)
              .HasPrincipalKey(i => i.ProductID);
        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .HasOne(i => i.ProductSupplier)
              .WithMany(i => i.ProductSupplies)
              .HasForeignKey(i => i.ProductSupplierID)
              .HasPrincipalKey(i => i.ProductSupplierID);
        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .HasOne(i => i.Product)
              .WithMany(i => i.ProductSupplies)
              .HasForeignKey(i => i.ProductID)
              .HasPrincipalKey(i => i.ProductID);


        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .Property(p => p.SalesDate)
              .HasColumnType("datetime");

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .Property(p => p.SupplyDate)
              .HasColumnType("datetime");

        builder.Entity<IyaElepoApp.Models.ConData.Customer>()
              .Property(p => p.CustomerID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.Customer>()
              .Property(p => p.CustomerTypeID)
              .HasPrecision(10, 0);

        builder.Entity<IyaElepoApp.Models.ConData.CustomerType>()
              .Property(p => p.CustomerTypeID)
              .HasPrecision(10, 0);

        builder.Entity<IyaElepoApp.Models.ConData.Product>()
              .Property(p => p.ProductID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.Product>()
              .Property(p => p.ProductTypeID)
              .HasPrecision(10, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .Property(p => p.ProductSaleID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .Property(p => p.CustomerID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .Property(p => p.ProductID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSale>()
              .Property(p => p.QuantityInLitres)
              .HasPrecision(18, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupplier>()
              .Property(p => p.ProductSupplierID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .Property(p => p.SupplyID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .Property(p => p.ProductSupplierID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .Property(p => p.ProductID)
              .HasPrecision(19, 0);

        builder.Entity<IyaElepoApp.Models.ConData.ProductSupply>()
              .Property(p => p.QuantityInLitres)
              .HasPrecision(18, 2);

        builder.Entity<IyaElepoApp.Models.ConData.ProductType>()
              .Property(p => p.ProductTypeID)
              .HasPrecision(10, 0);
        this.OnModelBuilding(builder);
    }


    public DbSet<IyaElepoApp.Models.ConData.Customer> Customers
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.CustomerType> CustomerTypes
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.Product> Products
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.ProductSale> ProductSales
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.ProductSupplier> ProductSuppliers
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.ProductSupply> ProductSupplies
    {
      get;
      set;
    }

    public DbSet<IyaElepoApp.Models.ConData.ProductType> ProductTypes
    {
      get;
      set;
    }
  }
}

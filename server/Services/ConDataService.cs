using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using IyaElepoApp.Data;

namespace IyaElepoApp
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportCustomersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCustomersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCustomersRead(ref IQueryable<Models.ConData.Customer> items);

        public async Task<IQueryable<Models.ConData.Customer>> GetCustomers(Query query = null)
        {
            var items = Context.Customers.AsQueryable();

            items = items.Include(i => i.CustomerType);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCustomersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCustomerCreated(Models.ConData.Customer item);
        partial void OnAfterCustomerCreated(Models.ConData.Customer item);

        public async Task<Models.ConData.Customer> CreateCustomer(Models.ConData.Customer customer)
        {
            OnCustomerCreated(customer);

            var existingItem = Context.Customers
                              .Where(i => i.CustomerID == customer.CustomerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Customers.Add(customer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(customer).State = EntityState.Detached;
                customer.CustomerType = null;
                throw;
            }

            OnAfterCustomerCreated(customer);

            return customer;
        }
        public async Task ExportCustomerTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customertypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customertypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCustomerTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/customertypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/customertypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCustomerTypesRead(ref IQueryable<Models.ConData.CustomerType> items);

        public async Task<IQueryable<Models.ConData.CustomerType>> GetCustomerTypes(Query query = null)
        {
            var items = Context.CustomerTypes.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCustomerTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCustomerTypeCreated(Models.ConData.CustomerType item);
        partial void OnAfterCustomerTypeCreated(Models.ConData.CustomerType item);

        public async Task<Models.ConData.CustomerType> CreateCustomerType(Models.ConData.CustomerType customerType)
        {
            OnCustomerTypeCreated(customerType);

            var existingItem = Context.CustomerTypes
                              .Where(i => i.CustomerTypeID == customerType.CustomerTypeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CustomerTypes.Add(customerType);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(customerType).State = EntityState.Detached;
                throw;
            }

            OnAfterCustomerTypeCreated(customerType);

            return customerType;
        }
        public async Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductsRead(ref IQueryable<Models.ConData.Product> items);

        public async Task<IQueryable<Models.ConData.Product>> GetProducts(Query query = null)
        {
            var items = Context.Products.AsQueryable();

            items = items.Include(i => i.ProductType);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductCreated(Models.ConData.Product item);
        partial void OnAfterProductCreated(Models.ConData.Product item);

        public async Task<Models.ConData.Product> CreateProduct(Models.ConData.Product product)
        {
            OnProductCreated(product);

            var existingItem = Context.Products
                              .Where(i => i.ProductID == product.ProductID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Products.Add(product);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(product).State = EntityState.Detached;
                product.ProductType = null;
                throw;
            }

            OnAfterProductCreated(product);

            return product;
        }
        public async Task ExportProductSalesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsales/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsales/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductSalesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsales/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsales/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductSalesRead(ref IQueryable<Models.ConData.ProductSale> items);

        public async Task<IQueryable<Models.ConData.ProductSale>> GetProductSales(Query query = null)
        {
            var items = Context.ProductSales.AsQueryable();

            items = items.Include(i => i.Customer);

            items = items.Include(i => i.Product);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductSalesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductSaleCreated(Models.ConData.ProductSale item);
        partial void OnAfterProductSaleCreated(Models.ConData.ProductSale item);

        public async Task<Models.ConData.ProductSale> CreateProductSale(Models.ConData.ProductSale productSale)
        {
            OnProductSaleCreated(productSale);

            var existingItem = Context.ProductSales
                              .Where(i => i.ProductSaleID == productSale.ProductSaleID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ProductSales.Add(productSale);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(productSale).State = EntityState.Detached;
                productSale.Customer = null;
                productSale.Product = null;
                throw;
            }

            OnAfterProductSaleCreated(productSale);

            return productSale;
        }
        public async Task ExportProductSuppliersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsuppliers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsuppliers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductSuppliersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsuppliers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsuppliers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductSuppliersRead(ref IQueryable<Models.ConData.ProductSupplier> items);

        public async Task<IQueryable<Models.ConData.ProductSupplier>> GetProductSuppliers(Query query = null)
        {
            var items = Context.ProductSuppliers.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductSuppliersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductSupplierCreated(Models.ConData.ProductSupplier item);
        partial void OnAfterProductSupplierCreated(Models.ConData.ProductSupplier item);

        public async Task<Models.ConData.ProductSupplier> CreateProductSupplier(Models.ConData.ProductSupplier productSupplier)
        {
            OnProductSupplierCreated(productSupplier);

            var existingItem = Context.ProductSuppliers
                              .Where(i => i.ProductSupplierID == productSupplier.ProductSupplierID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ProductSuppliers.Add(productSupplier);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(productSupplier).State = EntityState.Detached;
                throw;
            }

            OnAfterProductSupplierCreated(productSupplier);

            return productSupplier;
        }
        public async Task ExportProductSuppliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsupplies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsupplies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductSuppliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/productsupplies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/productsupplies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductSuppliesRead(ref IQueryable<Models.ConData.ProductSupply> items);

        public async Task<IQueryable<Models.ConData.ProductSupply>> GetProductSupplies(Query query = null)
        {
            var items = Context.ProductSupplies.AsQueryable();

            items = items.Include(i => i.ProductSupplier);

            items = items.Include(i => i.Product);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductSuppliesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductSupplyCreated(Models.ConData.ProductSupply item);
        partial void OnAfterProductSupplyCreated(Models.ConData.ProductSupply item);

        public async Task<Models.ConData.ProductSupply> CreateProductSupply(Models.ConData.ProductSupply productSupply)
        {
            OnProductSupplyCreated(productSupply);

            var existingItem = Context.ProductSupplies
                              .Where(i => i.SupplyID == productSupply.SupplyID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ProductSupplies.Add(productSupply);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(productSupply).State = EntityState.Detached;
                productSupply.ProductSupplier = null;
                productSupply.Product = null;
                throw;
            }

            OnAfterProductSupplyCreated(productSupply);

            return productSupply;
        }
        public async Task ExportProductTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/producttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/producttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/producttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/producttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductTypesRead(ref IQueryable<Models.ConData.ProductType> items);

        public async Task<IQueryable<Models.ConData.ProductType>> GetProductTypes(Query query = null)
        {
            var items = Context.ProductTypes.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductTypeCreated(Models.ConData.ProductType item);
        partial void OnAfterProductTypeCreated(Models.ConData.ProductType item);

        public async Task<Models.ConData.ProductType> CreateProductType(Models.ConData.ProductType productType)
        {
            OnProductTypeCreated(productType);

            var existingItem = Context.ProductTypes
                              .Where(i => i.ProductTypeID == productType.ProductTypeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ProductTypes.Add(productType);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(productType).State = EntityState.Detached;
                throw;
            }

            OnAfterProductTypeCreated(productType);

            return productType;
        }

        partial void OnCustomerDeleted(Models.ConData.Customer item);
        partial void OnAfterCustomerDeleted(Models.ConData.Customer item);

        public async Task<Models.ConData.Customer> DeleteCustomer(Int64? customerId)
        {
            var itemToDelete = Context.Customers
                              .Where(i => i.CustomerID == customerId)
                              .Include(i => i.ProductSales)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCustomerDeleted(itemToDelete);

            Context.Customers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCustomerDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCustomerGet(Models.ConData.Customer item);

        public async Task<Models.ConData.Customer> GetCustomerByCustomerId(Int64? customerId)
        {
            var items = Context.Customers
                              .AsNoTracking()
                              .Where(i => i.CustomerID == customerId);

            items = items.Include(i => i.CustomerType);

            var itemToReturn = items.FirstOrDefault();

            OnCustomerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.Customer> CancelCustomerChanges(Models.ConData.Customer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCustomerUpdated(Models.ConData.Customer item);
        partial void OnAfterCustomerUpdated(Models.ConData.Customer item);

        public async Task<Models.ConData.Customer> UpdateCustomer(Int64? customerId, Models.ConData.Customer customer)
        {
            OnCustomerUpdated(customer);

            var itemToUpdate = Context.Customers
                              .Where(i => i.CustomerID == customerId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(customer);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCustomerUpdated(customer);

            return customer;
        }

        partial void OnCustomerTypeDeleted(Models.ConData.CustomerType item);
        partial void OnAfterCustomerTypeDeleted(Models.ConData.CustomerType item);

        public async Task<Models.ConData.CustomerType> DeleteCustomerType(int? customerTypeId)
        {
            var itemToDelete = Context.CustomerTypes
                              .Where(i => i.CustomerTypeID == customerTypeId)
                              .Include(i => i.Customers)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCustomerTypeDeleted(itemToDelete);

            Context.CustomerTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCustomerTypeDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCustomerTypeGet(Models.ConData.CustomerType item);

        public async Task<Models.ConData.CustomerType> GetCustomerTypeByCustomerTypeId(int? customerTypeId)
        {
            var items = Context.CustomerTypes
                              .AsNoTracking()
                              .Where(i => i.CustomerTypeID == customerTypeId);

            var itemToReturn = items.FirstOrDefault();

            OnCustomerTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.CustomerType> CancelCustomerTypeChanges(Models.ConData.CustomerType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCustomerTypeUpdated(Models.ConData.CustomerType item);
        partial void OnAfterCustomerTypeUpdated(Models.ConData.CustomerType item);

        public async Task<Models.ConData.CustomerType> UpdateCustomerType(int? customerTypeId, Models.ConData.CustomerType customerType)
        {
            OnCustomerTypeUpdated(customerType);

            var itemToUpdate = Context.CustomerTypes
                              .Where(i => i.CustomerTypeID == customerTypeId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(customerType);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCustomerTypeUpdated(customerType);

            return customerType;
        }

        partial void OnProductDeleted(Models.ConData.Product item);
        partial void OnAfterProductDeleted(Models.ConData.Product item);

        public async Task<Models.ConData.Product> DeleteProduct(Int64? productId)
        {
            var itemToDelete = Context.Products
                              .Where(i => i.ProductID == productId)
                              .Include(i => i.ProductSupplies)
                              .Include(i => i.ProductSales)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductDeleted(itemToDelete);

            Context.Products.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductGet(Models.ConData.Product item);

        public async Task<Models.ConData.Product> GetProductByProductId(Int64? productId)
        {
            var items = Context.Products
                              .AsNoTracking()
                              .Where(i => i.ProductID == productId);

            items = items.Include(i => i.ProductType);

            var itemToReturn = items.FirstOrDefault();

            OnProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.Product> CancelProductChanges(Models.ConData.Product item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductUpdated(Models.ConData.Product item);
        partial void OnAfterProductUpdated(Models.ConData.Product item);

        public async Task<Models.ConData.Product> UpdateProduct(Int64? productId, Models.ConData.Product product)
        {
            OnProductUpdated(product);

            var itemToUpdate = Context.Products
                              .Where(i => i.ProductID == productId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(product);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterProductUpdated(product);

            return product;
        }

        partial void OnProductSaleDeleted(Models.ConData.ProductSale item);
        partial void OnAfterProductSaleDeleted(Models.ConData.ProductSale item);

        public async Task<Models.ConData.ProductSale> DeleteProductSale(Int64? productSaleId)
        {
            var itemToDelete = Context.ProductSales
                              .Where(i => i.ProductSaleID == productSaleId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductSaleDeleted(itemToDelete);

            Context.ProductSales.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductSaleDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductSaleGet(Models.ConData.ProductSale item);

        public async Task<Models.ConData.ProductSale> GetProductSaleByProductSaleId(Int64? productSaleId)
        {
            var items = Context.ProductSales
                              .AsNoTracking()
                              .Where(i => i.ProductSaleID == productSaleId);

            items = items.Include(i => i.Customer);

            items = items.Include(i => i.Product);

            var itemToReturn = items.FirstOrDefault();

            OnProductSaleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.ProductSale> CancelProductSaleChanges(Models.ConData.ProductSale item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductSaleUpdated(Models.ConData.ProductSale item);
        partial void OnAfterProductSaleUpdated(Models.ConData.ProductSale item);

        public async Task<Models.ConData.ProductSale> UpdateProductSale(Int64? productSaleId, Models.ConData.ProductSale productSale)
        {
            OnProductSaleUpdated(productSale);

            var itemToUpdate = Context.ProductSales
                              .Where(i => i.ProductSaleID == productSaleId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(productSale);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterProductSaleUpdated(productSale);

            return productSale;
        }

        partial void OnProductSupplierDeleted(Models.ConData.ProductSupplier item);
        partial void OnAfterProductSupplierDeleted(Models.ConData.ProductSupplier item);

        public async Task<Models.ConData.ProductSupplier> DeleteProductSupplier(Int64? productSupplierId)
        {
            var itemToDelete = Context.ProductSuppliers
                              .Where(i => i.ProductSupplierID == productSupplierId)
                              .Include(i => i.ProductSupplies)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductSupplierDeleted(itemToDelete);

            Context.ProductSuppliers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductSupplierDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductSupplierGet(Models.ConData.ProductSupplier item);

        public async Task<Models.ConData.ProductSupplier> GetProductSupplierByProductSupplierId(Int64? productSupplierId)
        {
            var items = Context.ProductSuppliers
                              .AsNoTracking()
                              .Where(i => i.ProductSupplierID == productSupplierId);

            var itemToReturn = items.FirstOrDefault();

            OnProductSupplierGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.ProductSupplier> CancelProductSupplierChanges(Models.ConData.ProductSupplier item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductSupplierUpdated(Models.ConData.ProductSupplier item);
        partial void OnAfterProductSupplierUpdated(Models.ConData.ProductSupplier item);

        public async Task<Models.ConData.ProductSupplier> UpdateProductSupplier(Int64? productSupplierId, Models.ConData.ProductSupplier productSupplier)
        {
            OnProductSupplierUpdated(productSupplier);

            var itemToUpdate = Context.ProductSuppliers
                              .Where(i => i.ProductSupplierID == productSupplierId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(productSupplier);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterProductSupplierUpdated(productSupplier);

            return productSupplier;
        }

        partial void OnProductSupplyDeleted(Models.ConData.ProductSupply item);
        partial void OnAfterProductSupplyDeleted(Models.ConData.ProductSupply item);

        public async Task<Models.ConData.ProductSupply> DeleteProductSupply(Int64? supplyId)
        {
            var itemToDelete = Context.ProductSupplies
                              .Where(i => i.SupplyID == supplyId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductSupplyDeleted(itemToDelete);

            Context.ProductSupplies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductSupplyDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductSupplyGet(Models.ConData.ProductSupply item);

        public async Task<Models.ConData.ProductSupply> GetProductSupplyBySupplyId(Int64? supplyId)
        {
            var items = Context.ProductSupplies
                              .AsNoTracking()
                              .Where(i => i.SupplyID == supplyId);

            items = items.Include(i => i.ProductSupplier);

            items = items.Include(i => i.Product);

            var itemToReturn = items.FirstOrDefault();

            OnProductSupplyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.ProductSupply> CancelProductSupplyChanges(Models.ConData.ProductSupply item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductSupplyUpdated(Models.ConData.ProductSupply item);
        partial void OnAfterProductSupplyUpdated(Models.ConData.ProductSupply item);

        public async Task<Models.ConData.ProductSupply> UpdateProductSupply(Int64? supplyId, Models.ConData.ProductSupply productSupply)
        {
            OnProductSupplyUpdated(productSupply);

            var itemToUpdate = Context.ProductSupplies
                              .Where(i => i.SupplyID == supplyId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(productSupply);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterProductSupplyUpdated(productSupply);

            return productSupply;
        }

        partial void OnProductTypeDeleted(Models.ConData.ProductType item);
        partial void OnAfterProductTypeDeleted(Models.ConData.ProductType item);

        public async Task<Models.ConData.ProductType> DeleteProductType(int? productTypeId)
        {
            var itemToDelete = Context.ProductTypes
                              .Where(i => i.ProductTypeID == productTypeId)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductTypeDeleted(itemToDelete);

            Context.ProductTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductTypeDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductTypeGet(Models.ConData.ProductType item);

        public async Task<Models.ConData.ProductType> GetProductTypeByProductTypeId(int? productTypeId)
        {
            var items = Context.ProductTypes
                              .AsNoTracking()
                              .Where(i => i.ProductTypeID == productTypeId);

            var itemToReturn = items.FirstOrDefault();

            OnProductTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.ConData.ProductType> CancelProductTypeChanges(Models.ConData.ProductType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductTypeUpdated(Models.ConData.ProductType item);
        partial void OnAfterProductTypeUpdated(Models.ConData.ProductType item);

        public async Task<Models.ConData.ProductType> UpdateProductType(int? productTypeId, Models.ConData.ProductType productType)
        {
            OnProductTypeUpdated(productType);

            var itemToUpdate = Context.ProductTypes
                              .Where(i => i.ProductTypeID == productTypeId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(productType);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterProductTypeUpdated(productType);

            return productType;
        }
    }
}

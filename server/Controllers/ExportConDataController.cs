using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IyaElepoApp.Data;

namespace IyaElepoApp
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;
        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/customers/csv")]
        [HttpGet("/export/ConData/customers/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/customers/excel")]
        [HttpGet("/export/ConData/customers/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/customertypes/csv")]
        [HttpGet("/export/ConData/customertypes/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomerTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCustomerTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/customertypes/excel")]
        [HttpGet("/export/ConData/customertypes/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomerTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCustomerTypes(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/products/csv")]
        [HttpGet("/export/ConData/products/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/products/excel")]
        [HttpGet("/export/ConData/products/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProducts(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/productsales/csv")]
        [HttpGet("/export/ConData/productsales/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSalesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProductSales(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/productsales/excel")]
        [HttpGet("/export/ConData/productsales/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSalesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProductSales(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/productsuppliers/csv")]
        [HttpGet("/export/ConData/productsuppliers/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSuppliersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProductSuppliers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/productsuppliers/excel")]
        [HttpGet("/export/ConData/productsuppliers/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSuppliersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProductSuppliers(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/productsupplies/csv")]
        [HttpGet("/export/ConData/productsupplies/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSuppliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProductSupplies(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/productsupplies/excel")]
        [HttpGet("/export/ConData/productsupplies/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductSuppliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProductSupplies(), Request.Query), fileName);
        }
        [HttpGet("/export/ConData/producttypes/csv")]
        [HttpGet("/export/ConData/producttypes/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProductTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/producttypes/excel")]
        [HttpGet("/export/ConData/producttypes/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportProductTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProductTypes(), Request.Query), fileName);
        }
    }
}

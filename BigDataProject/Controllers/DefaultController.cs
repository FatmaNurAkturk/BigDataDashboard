using BigDataProject.DAL.DTO;
using BigDataProject.DAL.Entities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data.SqlClient;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.Mime.MediaTypeNames;

namespace BigDataProject.Controllers
{
    public class DefaultController : Controller
    {
        private readonly string connectionString = "Server=LAPTOP-NP6T28C0\\SQLEXPRESS03;initial Catalog=CARPLATES;integrated security=true";


        public async Task<IActionResult> Index(string carName)
        {
                await using var connection = new SqlConnection(connectionString);           
                // Sorgular           
                var brandCount = await connection.QueryAsync<CountDTO>("SELECT COUNT(DISTINCT brand) AS count FROM PLATES;");
                var year = await connection.QueryAsync<CountDTO>("SELECT top 1 YEAR_ AS count FROM PLATES GROUP BY YEAR_ ORDER BY count DESC");
                var caseType = await connection.QueryAsync<CaseTypeDTO>("SELECT top 1 CASETYPE,Count(*) AS count FROM PLATES GROUP BY CASETYPE ORDER BY count DESC");
                var color = await connection.QueryAsync<ColorDTO>("SELECT top 1 COLOR,Count(*) AS count FROM PLATES GROUP BY COLOR ORDER BY count DESC");
                var diesel = await connection.QueryAsync<CountDTO>("select count(*) as count from PLATES where Fuel='Dizel'");
                var petrol = await connection.QueryAsync<CountDTO>("select count(*) as count from PLATES where Fuel='Benzin'");
                var model = await connection.QueryAsync<ModelDTO>("SELECT top 1 MODEL,Count(*) AS count FROM PLATES GROUP BY MODEL ORDER BY count DESC");
                var brand = await connection.QueryAsync<BrandDTO>("SELECT top 1 BRAND,Count(*) AS count FROM PLATES GROUP BY BRAND ORDER BY count DESC");
                // Atamalar            
                ViewBag.brandCount = brandCount.Select(x => x.Count).FirstOrDefault();
                ViewBag.year = year.Select(x => x.Count).FirstOrDefault();
                ViewBag.CaseType = caseType.Select(x => x.CASETYPE).FirstOrDefault();
                ViewBag.Color = color.Select(x => x.Color).FirstOrDefault();
                ViewBag.diesel = diesel.Select(x => x.Count).FirstOrDefault();
                ViewBag.petrol = petrol.Select(x => x.Count).FirstOrDefault();
                ViewBag.model = model.Select(x => x.MODEL).FirstOrDefault();
                ViewBag.brand = brand.Select(x => x.Brand).FirstOrDefault();                       
               
            
            return View();
        }     


        public async Task<IActionResult> Search(string keyword)
        {

            string query = @"
            SELECT TOP 1000 BRAND,COLOR, SUBSTRING(PLATE, 1, 2) AS PlatePrefix, SHIFTTYPE, FUEL
            FROM PLATES
            WHERE BRAND LIKE '%' + @Keyword + '%'
               OR COLOR LIKE '%' + @Keyword + '%'
               OR PLATE LIKE '%' + @Keyword + '%'
               OR SHIFTTYPE LIKE '%' + @Keyword + '%'
               OR FUEL LIKE '%' + @Keyword + '%'
        ";

            await using var connection = new SqlConnection(connectionString);
            connection.Open();

            //sorguyu çalıştırmak ve sonuç alamak için 
            // "SearchConclusion" sınıfı, sorgu sonuçlarıyla eşleşen verileri temsil eden bir model sınıfı olmalıdır.
            var searchResults = await connection.QueryAsync<SearchConclusion>(query, new { Keyword = keyword });

            // josn şeklinde  JsonResult" tipinde sonuç döndürü
            return Json(searchResults);

        }
    }
}

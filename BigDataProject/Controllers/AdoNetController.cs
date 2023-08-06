using BigDataProject.DAL.DTO;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

namespace BigDataProject.Controllers
{
    public class AdoNetController : Controller
    {
        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-NP6T28C0\\SQLEXPRESS03;Initial Catalog=CARPLATES;Integrated Security=True");

        public IActionResult Index(string carName)
        {           
                brandCount();
                year();
                brand();
                caseType();
                color();
                diesel();
                petrol();
                model();                                
            return View();
        }

        public async Task<IActionResult> Search(string carName)
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM PLATES WHERE BRAND=@p1", connection);
            komut.Parameters.AddWithValue("@p1", carName);
            SqlDataReader dr2 = komut.ExecuteReader();
            List<GetPlatesDTO> cars = new List<GetPlatesDTO>();
            while (dr2.Read())
            {
                GetPlatesDTO car = new GetPlatesDTO()
                {
                    ID = Convert.ToInt32(dr2["Id"]),
                    BRAND = dr2["BRAND"].ToString(),
                    CITYNR = dr2["CITYNR"].ToString(),
                    COLOR = dr2["COLOR"].ToString(),
                    MODEL = dr2["MODEL"].ToString(),
                    PLATE = dr2["PLATE"].ToString(),
                    TITLE = dr2["TITLE"].ToString(),
                    YEAR_ = dr2["YEAR_"].ToString(),
                };

                cars.Add(car);
            }
            dr2.Close();
            connection.Close();
            return Json(cars);
        }

        public void brandCount()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT COUNT(DISTINCT brand) AS count FROM PLATES;", connection);          
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.brandCount = dr2["count"];
            }
            dr2.Close();
            connection.Close();
        }

        public void year()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT top 1 YEAR_ AS count FROM PLATES GROUP BY YEAR_ ORDER BY count DESC", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.year = dr2["count"];
            }
            dr2.Close();
            connection.Close();
        }
        public void caseType()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT top 1 CASETYPE,Count(*) AS count FROM PLATES GROUP BY CASETYPE ORDER BY count DESC", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.CaseType = dr2["CASETYPE"];
            }
            dr2.Close();
            connection.Close();
        }
        public void color()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT top 1 COLOR,Count(*) AS count FROM PLATES GROUP BY COLOR ORDER BY count DESC", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.color = dr2["COLOR"];
            }
            dr2.Close();
            connection.Close();
        }
        public void diesel()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("select count(*) as count from PLATES where Fuel='Dizel'", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.diesel = dr2["count"];
            }
            dr2.Close();
            connection.Close();
        }
        public void petrol()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("select count(*) as count from PLATES where Fuel='Dizel'", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.petrol = dr2["count"];
            }
            dr2.Close();
            connection.Close();
        }
        public void model()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT top 1 MODEL,Count(*) AS count FROM PLATES GROUP BY MODEL ORDER BY count DESC", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.model = dr2["MODEL"];
            }
            dr2.Close();
            connection.Close();
        }
        public void brand()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("SELECT top 1 BRAND,Count(*) AS count FROM PLATES GROUP BY BRAND ORDER BY count DESC", connection);
            SqlDataReader dr2 = komut.ExecuteReader();
            if (dr2.Read())
            {
                ViewBag.brand = dr2["BRAND"];
            }
            dr2.Close();
            connection.Close();
        }
    }
}

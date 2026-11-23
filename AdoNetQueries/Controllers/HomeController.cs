using AdoNetQueries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace AdoNetQueries.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config) => _config = config;

        public IActionResult Index()
        {
            var dane = new DataTable();

            using var conn = new SqlConnection(_config.GetConnectionString("MyDbConnect"));
            using var cmd = new SqlCommand("select * from studenci", conn);

            using var adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dane);

            return View(dane);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(string imie, string nazwisko, DateTime data_urodzenia, string? miasto, int rok_studiow)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("MyDbConnect"));
            using var cmd = new SqlCommand("insert into studenci (imie, nazwisko, data_urodzenia, miasto, rok_studiow)" +
                "values (@imie, @nazwisko, @data, @miasto, @rok)", conn);

            cmd.Parameters.AddWithValue("@imie", imie);
            cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
            cmd.Parameters.AddWithValue("@data", data_urodzenia);
            cmd.Parameters.AddWithValue("@miasto", miasto);
            cmd.Parameters.AddWithValue("@rok", rok_studiow);

            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Loan_Finance_Client_final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Loan_Finance_Client_final.Controllers
{
    public class AdminsController : Controller
    {
        IConfiguration _configuration;
        public string BaseURL;

        public AdminsController(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseURL = _configuration.GetValue<string>("BaseURL");
        }
        // GET: AdminsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Inputs are invalid";
                return View();
            }
            List<Admin> admins = await GetAdmin();

            var obj = admins.Where(a => a.AdminName.Equals(admin.AdminName)
                    && a.Password.Equals(admin.Password)).FirstOrDefault();

            if (obj != null)
            {
                HttpContext.Session.SetString("AdminName", obj.AdminName.ToString());
                return RedirectToAction("Menu", "Admins");
            }//if
            else
            {
                ViewBag.Message = "Admin Email and Password Are Wrong";
                return View();
            }//else
        }



        public async Task<List<Admin>> GetAdmin()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);
            string JsonStr = await client.GetStringAsync(BaseURL + "/api/admins");
            var result = JsonConvert.DeserializeObject<List<Admin>>(JsonStr);
            return result;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }


    }
}

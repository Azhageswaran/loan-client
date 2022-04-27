using Loan_Finance_Client_final.Models;
using Loan_Finance_Client_final.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Loan_Finance_Client_final.Controllers
{
    public class UserController : Controller
    {
        IConfiguration _configuration;
        string BaseURL;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseURL = _configuration.GetValue<string>("BaseURL");
        }//Ctor

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            try
            {
                string aadharUniqueFileName=null;
                string panUniqueFileName=null;
                string AadharUploadPath = "AadharPanFile/Aadhar";
                string PanUploadFolder = "AadharPanFile/Pan";
                Console.WriteLine(userViewModel.AadharFile.FileName);
                if (userViewModel.PanFile != null && userViewModel.AadharFile != null)
                {
                    Console.WriteLine("Inside IF");
                    aadharUniqueFileName = Guid.NewGuid().ToString() + "-" + userViewModel.AadharFile.FileName;
                    panUniqueFileName = Guid.NewGuid().ToString() + "-" + userViewModel.PanFile.FileName;
                    string AadharFilePath = Path.Combine(AadharUploadPath, aadharUniqueFileName + Path.GetExtension(userViewModel.AadharFile.FileName));
                    string PanFilePath = Path.Combine(PanUploadFolder, panUniqueFileName + Path.GetExtension(userViewModel.PanFile.FileName));

                    userViewModel.AadharFile.CopyTo(new FileStream(AadharFilePath, FileMode.Create));
                    userViewModel.PanFile.CopyTo(new FileStream(PanFilePath, FileMode.Create));



                    User user = new User()
                    {
                        UserName = userViewModel.UserName,
                        Address = userViewModel.Address,
                        Age = userViewModel.Age,
                        Email = userViewModel.Email,
                        Aadhar = userViewModel.Aadhar,
                        Pan = userViewModel.Pan,
                        Password = userViewModel.Password,
                        PanFileName = PanFilePath,
                        AadharFileName = AadharFilePath,
                        PhoneNumber = userViewModel.PhoneNumber,
                        Salary = userViewModel.Salary,
                        UserId = userViewModel.UserId,

                    };
                    User recievedUserInfo = new User();
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    var httpClient = new HttpClient(clientHandler);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(BaseURL + "/api/Users", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        recievedUserInfo = JsonConvert.DeserializeObject<User>(apiResponse);
                        if (recievedUserInfo != null)
                        {
                            return RedirectToAction("Login", "User");
                        }//if
                    }//using

                    ViewBag.Message = "User Info is No COrrect !!! Please Try again";
                    return View();
                    //return RedirectToAction(nameof(Index));
                }//try
                else
                {
                    return View();
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Inputs are invalid";
                return View();
            }
            List<User> users = await GetUser();

            var obj = users.Where(a => a.Email.Equals(user.Email)
                    && a.Password.Equals(user.Password)).FirstOrDefault();

            if (obj != null)
            {
                HttpContext.Session.SetString("EmailID", obj.Email.ToString());
                HttpContext.Session.SetString("ID", obj.UserId.ToString());
                return RedirectToAction("MainMenu", "User");
            }//if
            else
            {
                ViewBag.Message = "Admin Email and Password Are Wrong";
                return View();
            }//else
        }



        public async Task<List<User>> GetUser()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);
            string JsonStr = await client.GetStringAsync(BaseURL + "/api/Users");
            var result = JsonConvert.DeserializeObject<List<User>>(JsonStr);
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

        public IActionResult MainMenu()
        {
            return View();
        }
      
        public async Task<ActionResult> ViewUser()
        {
            List<User> users = await GetYourUser();
            //int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            //List<User> yourUser = users.Where(user => user.UserId == id).ToList();
            return View(users);
        }

        public async Task<List<User>> GetYourUser()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);
            string JsonStr = await client.GetStringAsync(BaseURL + "/api/Users");
            var result = JsonConvert.DeserializeObject<List<User>>(JsonStr);
            return result;
        }
    }
}

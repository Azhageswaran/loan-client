using Loan_Finance_Client_final.Models;
using Loan_Finance_Client_final.Models.ForeignKeyMap;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Loan_Finance_Client_final.Controllers
{
    public class LoansController : Controller
    {
        IConfiguration _configuration;
        string BaseURL;

        public LoansController(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseURL = _configuration.GetValue<string>("BaseURL");
        }//Ctor


        // GET: LoansController
        public async Task<ActionResult> Index()
        {
            List<Loan> loans = await GetLoan();
            List<User> users = await GetUser();
            List < LoanWithUser >  loanWithUsers= new List<LoanWithUser>();
            foreach(var item in loans)
            {
                User user = users.FirstOrDefault(u=>u.UserId == item.UserId);
                LoanWithUser loanWithUser = new LoanWithUser()
                {
                    UserId = item.UserId,
                    UserName = user.UserName,
                    PanFileName = user.PanFileName,
                    AadharFileName = user.AadharFileName,
                    LoanID = item.LoanID,
                    LoanAmount = item.LoanAmount,
                    MonthOfEmi = item.MonthOfEmi,
                    OnRoadPrice = item.OnRoadPrice,
                    Message = item.Message,
                    Status = item.Status


                };
                loanWithUsers.Add(loanWithUser);
            }

            return View(loanWithUsers);
        }

        public async Task<ActionResult> ViewYourLoans()
        {
            List<Loan> loans = await GetLoan();
            int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            List<Loan> yourLoan = loans.Where(loan => loan.UserId == id).ToList();
            return View(yourLoan);
        }

        public async Task<List<Loan>> GetLoan()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);
            string JsonStr = await client.GetStringAsync(BaseURL + "/api/Loans");
            var result = JsonConvert.DeserializeObject<List<Loan>>(JsonStr);
            return result;
        }

        // GET: LoansController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: LoansController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoansController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Loan loan)
        {
            try
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
                loan.UserId = id;
                Loan recievedLoan = new Loan();
                HttpClientHandler clientHandler = new HttpClientHandler();
                var httpClient = new HttpClient(clientHandler);
                StringContent content = new StringContent(JsonConvert.SerializeObject(loan), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(BaseURL + "/api/loans", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    recievedLoan = JsonConvert.DeserializeObject<Loan>(apiResponse);
                    if (recievedLoan != null)
                    {
                        return RedirectToAction("ViewYourLoans", "Loans");
                    }//if
                }//using

                ViewBag.Message = "User Info is No COrrect !!! Please Try again";
                return View();
                //return RedirectToAction(nameof(Index));
            }//try
           
            
            catch
            {
                return View();
            }
        }


        public async Task<ActionResult >DownloadAadhar(string fileName)
        {
            if (fileName == null)
                return Content("filename not present");
            

            var path = fileName; 

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            return File(memory, contentType, Path.GetFileName(path));
        }

        public async Task<ActionResult> DownloadPan(string fileName)
        {
            if (fileName == null)
                return Content("filename not present");
            

            var path = fileName;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            return File(memory, contentType, Path.GetFileName(path));
        }

    



    // GET: LoansController/Edit/5
    public async Task<ActionResult> Edit(int id)
        {
            List<Loan> loans = await GetLoan();
                return View(loans.FirstOrDefault(l => l.LoanID == id));
        }

        // POST: LoansController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,Loan loan)
        {
            loan.UserId = id;
            //var accessToken = HttpContext.Session.GetString("Email");

            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpsClient = new HttpClient(clientHandler);

            StringContent content = new StringContent(JsonConvert.SerializeObject(loan), Encoding.UTF8, "application/json");

            using (var response = await httpsClient.PutAsync(BaseURL + "/api/loans/" + id, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (apiResponse != null)
                    return RedirectToAction("Index");
                else
                    return View();
            }//using
            //try
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: LoansController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoansController/Delete/5
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

        public async Task<List<User>> GetUser()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);
            string JsonStr = await client.GetStringAsync(BaseURL + "/api/Users");
            var result = JsonConvert.DeserializeObject<List<User>>(JsonStr);
            return result;
        }

    }
}

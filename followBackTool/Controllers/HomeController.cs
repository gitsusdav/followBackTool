using followBackTool.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Web;


namespace followBackTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lang = Request.Cookies["language"];
            if (!string.IsNullOrEmpty(lang))
            {
                CultureInfo.CurrentCulture = new CultureInfo(lang);
                CultureInfo.CurrentUICulture = new CultureInfo(lang);
            }
            ViewData["Language"] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            List<string> notMutualsUsernames = new List<string>();

            if (!IsValid(file))
            {
                ViewBag.Message = "Por favor selecciona un archivo valido";
                return View();
            }

            else
            {
                notMutualsUsernames = ReadZip(file);

                if (notMutualsUsernames.Count == 0)
                {
                    ViewBag.Message = "Por favor selecciona un archivo valido";
                    return View();
                }
               
             
                ViewBag.Message = "Lo hicimos, we did it!";
            }

            TempData["notMutalsUsernames"] = notMutualsUsernames;
            User userTarget = new User();
            userTarget.notMutuals = notMutualsUsernames;
            TempData["User"] = JsonConvert.SerializeObject(userTarget);
            return RedirectToAction("Resultados");
        }

       

        private static List<string> ReadZip(IFormFile file)
        {

            
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    List<string> following = new List<string>();
                    List<string> notMutuals = new List<string>();
                try
                {

                    using (ZipArchive archive = new ZipArchive(stream))
                    {
                        // Acceder directamente a followers_1.json
                        var followersEntry = archive.GetEntry("connections/followers_and_following/followers_1.json");
                        var followingEntry = archive.GetEntry("connections/followers_and_following/following.json");
                        if (followingEntry != null)
                        {
                            using (StreamReader reader = new StreamReader(followingEntry.Open()))
                            {
                                string followingContent = reader.ReadToEnd();
                                var followingRoot = JsonConvert.DeserializeObject<Root>(followingContent);

                                foreach (var item in followingRoot.relationships_following)
                                {
                                    var value = item.string_list_data[0].value;
                                    following.Add(value);
                                }
                            }
                        }

                        if (followersEntry != null)
                        {
                            using (StreamReader reader = new StreamReader(followersEntry.Open()))
                            {
                                string followersContent = reader.ReadToEnd();

                                // this people doesnt follow u back ;)

                                Console.WriteLine("this people doesnt follow u back: ");
                                foreach (var item in following)
                                {
                                    if (!followersContent.Contains(item))
                                    {
                                        Console.WriteLine(item);
                                        notMutuals.Add(item);
                                    }
                                }
                            }
                        }
                        return notMutuals;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Archivo no valido");
                }
                return notMutuals;
                }
        }




        private bool IsValid(IFormFile file)
        {
            if (file == null || file.Length < 0 || file.Length > 2500000 || !file.FileName.Contains(".zip") || !file.FileName.Contains("instagram")) {
                return false;
            }
            else {
                return true;
            }


        }

        public IActionResult About()
        {
            var lang = Request.Cookies["language"];
            if (!string.IsNullOrEmpty(lang))
            {
                CultureInfo.CurrentCulture = new CultureInfo(lang);
                CultureInfo.CurrentUICulture = new CultureInfo(lang);
            }
            ViewData["Language"] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            return View();
        }

        public IActionResult Resultados() 
        {
            var lang = Request.Cookies["language"];
            if (!string.IsNullOrEmpty(lang))
            {
                CultureInfo.CurrentCulture = new CultureInfo(lang);
                CultureInfo.CurrentUICulture = new CultureInfo(lang);
            }
            ViewData["Language"] = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (TempData["User"] != null)
            {
                User user = JsonConvert.DeserializeObject<User>((string)TempData["User"]);
                return View(user);
            }

            else
            {
                return RedirectToAction("Index");
            }
            
          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

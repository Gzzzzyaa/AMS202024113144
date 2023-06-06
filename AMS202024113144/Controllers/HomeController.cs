using AMS202024113144.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AMS202024113144.Models;
using static System.Reflection.Metadata.BlobBuilder;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AMS202024113144.Controllers
{
    public class HomeController : Controller
    {
        private readonly ManageDbContext _context;
        private IList<Asset> assets;
        private string _path; //图片路径变项
        public HomeController(ManageDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }
        public IActionResult Query(string choice, string keyword)
        {

            if(choice == "0")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                    .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                    .ToList();
            }
            else if(choice == "1")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetTitle.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "2")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetSpecification.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "3")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.AssetPrice.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "4")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.Location.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "5")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.Category.CategoryName.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            else if (choice == "6")
            {
                assets = _context.Assets.OrderBy(b => b.AssetId)
                .Where(b => b.MidNavigation.Name.Contains(keyword))
                .Include(b => b.Category).AsNoTracking()
                    .Include(b => b.MidNavigation).AsNoTracking()
                    .OrderBy(b => b.AssetId)
                .ToList();
            }
            //使用LINQ扩充方法


            return View(assets);
        }

        public IActionResult Index(string cat)
        {
            if (cat != null) //找特定类别相片,最新提交的8张相片,作为首页显示用
            {
                assets = _context.Assets.OrderBy(p => p.AssetId)
                .Where(p => p.Category.CategoryName.Equals(cat))
                .Include(p => p.Category).AsNoTracking()
                .Include(p => p.MidNavigation).AsNoTracking()
                .OrderByDescending(p => p.PurchaseTime)
                .Take(8)
                .ToList();
            }
            else //找最新提交的8张相片,作为首页显示用
            {
                assets = _context.Assets.OrderBy(p => p.AssetId)
                .Include(p => p.Category).AsNoTracking()
                .Include(p => p.MidNavigation).AsNoTracking()
                .OrderByDescending(p => p.PurchaseTime)
                .Take(8)
                .ToList();
            }
            var catNames = _context.Categories.Select(c => c.CategoryName).ToList();
            ViewBag.catNames = catNames;
            return View(assets);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(int mid, string pwd)
        {
            //取得会员对象
            //var member = new MemberList().GetMember(uid, pwd);
            Member member = _context.Members.FirstOrDefault(m => m.Mid == mid && m.Password == pwd);
            if (member != null)
            {
                //建立身份声明
                IList<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, member.Mid.ToString()),
                    new Claim(ClaimTypes.Role, member.Role.Trim())
                    };
                //建立身份识别对象,并指定账号与角色
                var claimsIndentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                //进行登录动作,并带入身份识别对象
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIndentity),
                authProperties);
                //重定向至会员页
                TempData["Message"] = member.Role.ToString();
                return RedirectToAction("Index");

            }
            ViewBag.Message = "Wrong account or password!";
            return View("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}

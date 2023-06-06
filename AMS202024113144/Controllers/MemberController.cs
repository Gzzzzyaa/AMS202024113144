using AMS202024113144.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace AMS202024113144.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly ManageDbContext _context;
        private IList<Member> members;
        private string _path; //图片路径变项
        public MemberController(ManageDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }
        //PhotoAdmin Action:相片管理页面
        public IActionResult MemberIndex()
        {
            members = _context.Members.OrderBy(p => p.Mid)
            .Include(b => b.DidNavigation).AsNoTracking()
            .ToList();
            return View(members);
        }
        public IActionResult Query(string choice, string keyword)
        {

            if (choice == "0")
            {
                members = _context.Members.OrderBy(b => b.Mid)
                    .Include(b => b.DidNavigation).AsNoTracking()
                .ToList();
                return View(members);
            }
            else if (choice == "1")
            {
                members = _context.Members.OrderBy(b => b.Mid)
                .Where(b => b.Name.Contains(keyword))
                .Include(b => b.DidNavigation).AsNoTracking()
                .OrderBy(b => b.Mid)
                .ToList();
            }
            else if (choice == "2")
            {
                members = _context.Members.OrderBy(b => b.Mid)
                .Where(b => b.Phone.Contains(keyword))
                .Include(b => b.DidNavigation).AsNoTracking()
                .OrderBy(b => b.Mid)
                .ToList();
            }
            else if (choice == "3")
            {
                members = _context.Members.OrderBy(b => b.Mid)
                .Where(b => b.Role.Contains(keyword))
                .Include(b => b.DidNavigation).AsNoTracking()
                .OrderBy(b => b.Mid)
                .ToList();
            }
            else if (choice == "4")
            {
                members = _context.Members.OrderBy(b => b.Mid)
                .Where(b => b.DidNavigation.Dname.Contains(keyword))
                .Include(b => b.DidNavigation).AsNoTracking()
                .OrderBy(b => b.Mid)
                .ToList();
            }
            //使用LINQ扩充方法


            return View(members);
        }
        //删除相片

        public IActionResult Delete(int id)
        {
            var member = _context.Members.FirstOrDefault(b => b.Mid == id);
            _context.Members.Remove(member);
            _context.SaveChanges();
            return RedirectToAction("MemberIndex"); //重定向到相片管理页
        }


        public IActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Members.Add(member);
                    _context.SaveChanges();
                    TempData["Result"] = "员工添加成功!";
                    //添加成功则重定向到Index动作方法,显示首页
                    return RedirectToAction("MemberIndex");

                }
                catch (Exception ex)
                {
                    TempData["Result"] = "员工添加失败!";
                }
            }
            return View(member);
        }
        //删除图书

        public IActionResult Edit(int id)
        {
            var member = _context.Members.FirstOrDefault(b => b.Mid == id);
            return View(member);
        }

        //修改图书
        [HttpPost]
        public IActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int mid = member.Mid;
                    var temp = _context.Members.FirstOrDefault(b => b.Mid == mid);
                    temp.Name = member.Name;
                    temp.Password = member.Password;
                    temp.Phone = member.Phone;
                    temp.Role = member.Role;
                    temp.Did = member.Did;
                    _context.SaveChanges();
                    TempData["Result"] = "员工修改成功!";
                    return RedirectToAction("MemberIndex");
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "员工修改失败!";
                }
            }
            return View(member);
        }

    }
}

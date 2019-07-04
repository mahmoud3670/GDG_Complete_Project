using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Controllers
{
    public class GlobalController : Controller

    {
        public GDGContext _GDGContext;
        public IHostingEnvironment _env;
        Random random = new Random();

        public GlobalController(GDGContext gDGContext, IHostingEnvironment env)
        {
            _GDGContext = gDGContext;
            _env = env;
        }

        //admin session after action
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
        //admin session before action
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
            
        //    //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))||
        //    //    string.IsNullOrEmpty(HttpContext.Session.GetString("Password")))
        //    //{
        //    //    context.Result = new RedirectResult("/Home/Login");
        //    //}
        //    //else
        //    //{
        //    //    string UserName = HttpContext.Session.GetString("UserName");
        //    //    string Password = HttpContext.Session.GetString("Password");
        //    //    var user = _GDGContext.Employees.AsNoTracking().Where(x => x.EmpUserName == UserName &&x.EmpPassword == Password).Include(x => x.EmpInfoNavigation).Include(x=>x.EmpDepartmentNavigation).ToList().First();
        //    //    if (user != null)
        //    //    {
        //    //        if (user.EmpPostion == user.Super)

        //    //        {
        //    //            ViewBag.postion ="1";
                    
        //    //        } else if(user.EmpPostion == user.Manger)
        //    //        {
        //    //            ViewBag.postion ="2";
        //    //        }
        //    //        else
        //    //        {
        //    //             context.Result = new RedirectResult("/Home/Login");
        //    //        }
        //    //        ViewBag.user = user;

        //    //    }
        //    //    else
        //    //    {
        //    //        context.Result = new RedirectResult("/Home/Login");
        //    //    }
        //    //}

        //}

        public string saveImg(IFormFileCollection file,string name)
        {
            //uplode img
            int lastIndex = file[0].FileName.LastIndexOf(".");
            int fileNameLength = file[0].FileName.Length;
            int NumberOfChar = fileNameLength - lastIndex;
            string extension = file[0].FileName.Substring(lastIndex, NumberOfChar);
            string path = _env.ContentRootPath + "/wwwroot/img/"+name+"/";

            //save data in db
            string ImgName = (random.Next(1000000).ToString()) + "_" + (DateTime.Now.ToString("ddMMyyyy-hhmmss")) + extension;
            //save img in harddisk
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            using (FileStream fs = System.IO.File.Create(path + ImgName))
            {
                file[0].CopyTo(fs);
                fs.Flush();
            }
            return ImgName;
        }

      
    }
}
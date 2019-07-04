using System.Linq;
using GDG_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Common
{
    public class Policy : ISessionTracing

    {
        private IHttpContextAccessor _httpContextAccessor;
        private GDGContext _GDGContext;

        public Policy(IHttpContextAccessor httpContextAccessor, GDGContext gDGContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _GDGContext = gDGContext;
        }
       
       

        public bool Login(string Username, string Password)
        {
            var user = _GDGContext.Employees.AsNoTracking().Where(x => x.EmpUserName == Username && x.EmpPassword == Password&&x.EmpActive==x.active).ToList().FirstOrDefault();
            if (user != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserName", Username);
                _httpContextAccessor.HttpContext.Session.SetString("Password", Password);
                return true;
            }
            else{
                return false;
            }
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.SetString("UserName", "");
            _httpContextAccessor.HttpContext.Session.SetString("Password", "");

        }

        //get user data 
        public Employees Authorization()
        {
            string userName = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            string userPassword = _httpContextAccessor.HttpContext.Session.GetString("Password");
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword))
            {
                return null;
              //  context.Result = new RedirectResult("/Home/Login");
            }
            else
            {
                //string UserName = HttpContext.Session.GetString("UserName");
                //string Password = HttpContext.Session.GetString("Password");
                var user = _GDGContext.Employees.AsNoTracking().Where(x => x.EmpUserName == userName && x.EmpPassword == userPassword && x.EmpActive == x.active).Include(x => x.EmpInfoNavigation).Include(x=>x.EmpDepartmentNavigation).ToList().FirstOrDefault();
              
                if (user == null)
                {
                    return null;

                }
                else
                {
                    
                    return user;
                   // context.Result = new RedirectResult("/Home/Login");
                }
            }
        }

        public string RoutPage()
        {
            var userCrdantial = Authorization();
            if (userCrdantial != null)
            {
                if (userCrdantial.EmpPostion == userCrdantial.admin)
                {
                    return "/News/Index";
                }
                else if (userCrdantial.EmpPostion == userCrdantial.accounting)
                {
                    return "/Employee/Details/" + userCrdantial.EmpId;
                }
                else if (userCrdantial.EmpPostion == userCrdantial.HR)
                {
                    return "/Employee/Index";
                }
                else if (userCrdantial.EmpPostion == userCrdantial.worker)
                {
                    return "/Employee/Details/"+ userCrdantial.EmpId;
                }
                else if (userCrdantial.EmpPostion == userCrdantial.dataEntry)
                {
                    return "/Employee/Details/" + userCrdantial.EmpId;
                }
              

            }
            return "/Home/Login";
        }
        public void LogEventError(string EventName, int EventActor, string EventReport)
        {
            LogEvent log = new LogEvent();
            log.EventName = EventName;
            log.EventActor = EventActor;
            log.EventReport = EventReport;
            _GDGContext.LogEvent.Add(log);
            _GDGContext.SaveChanges();
        }


    }
}

using GDG_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDG_Project.Common
{
  public interface ISessionTracing
    {
        //Login
        bool Login(string Username,  string Password);
        //logout
        void Logout();
        
        //Prmsion
        Employees Authorization();

        string RoutPage();
        void LogEventError(string EventName, int EventActor, string EventReport);
    }
}

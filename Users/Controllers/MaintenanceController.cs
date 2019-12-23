using DataLibrary.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Maintennce;
using Users.Models;

namespace Users.Controllers
{
    public class MaintenanceController:Controller
    {
        AppIdentityDbContext context = null;

        public MaintenanceController(AppIdentityDbContext cx)
        {
            context = cx;
        }


        public ViewResult GenerateCode()
        {
            ViewBag.message = "AAAAAAAAAAAAAAAAAAAA";
            return View();
        }

        [HttpPost]
        public ViewResult GenerateCode(string Id)
        {
            CodeGenerator gen = new CodeGenerator(context);
            int i = gen.GenerateCodes(8);



            ViewBag.message = i.ToString();
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ICompany _company;
        private IHelp _help;


        public HomeController(ICompany company, IHelp help)
        {
            _company = company;
            _help = help;

        }

        [Authorize]
        public IActionResult HomeIndex()
        {
            var user = this.User;
            var result = _company.GetStatistics(ModelState, user);
            return View(result);
        }  
        public IActionResult help()
        {
            var result = _help.Get(ModelState);
            return View(result);
        }    
        public IActionResult Privacy()
        {
            var result = _help.Get1(ModelState);
            return View(result);
        }     
    
        public IActionResult AddHelp(Helps model)
        {
            var result = _help.Add(ModelState, model);
            return RedirectToAction("help","Home");
        }
        public IActionResult AddPrivacy(Privacy model)
        {
            var result = _help.Add1(ModelState, model);
            return RedirectToAction("Privacy", "Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCSimpleMessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private IMessageContainer messageContainer;

        public IActionResult Index(IMessageContainer messageContainer)
        {
            this.messageContainer = messageContainer;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
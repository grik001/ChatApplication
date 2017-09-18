using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatWebApplication.Front.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult CustomerSupport()
        {
            return View();
        }

        public ActionResult AdminSupport()
        {
            return View();
        }
    }
}
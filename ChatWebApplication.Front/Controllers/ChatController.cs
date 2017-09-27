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
            this.Response.Cookies["ClientID"].Value = "be628a64-0b66-4724-bcae-8f3e3805ad8e";
            return View();
        }

        public ActionResult AdminSupport()
        {
            this.Response.Cookies["AdminID"].Value = "ebd3ffd3-93f1-4ad3-8cb4-1870a6863ac0";
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
namespace Blog.Controllers
{ 

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
            private SampleDBContext model = new SampleDBContext();

        public ActionResult Login(string name, string password)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    Random random = new Random();
                    byte[] randomData = new byte[sizeof(long)];
                    random.NextBytes(randomData);
                    string newNonce = BitConverter.ToUInt64(randomData, 0).ToString("x16");
                    Session["Nonce"] = newNonce;
                    return View(model:newNonce);

                }
                Administrator administrator = model.Administrators.Where(x => x.Name == name).FirstOrDefault();
                string nonce = Session["Nonce"] as string;
                if(administrator==null || string.IsNullOrWhiteSpace(nonce))
                {
                    return RedirectToAction("Index", "Posts");
                }
               
                Session["IsAdmin"] = (administrator.password.ToLower() == password.ToLower());
                return RedirectToAction("Index", "Posts");
            }
        
            public ActionResult Logout()
            {
                Session["Nonce"] = null;
                Session["IsAdmin"] = null;
                return RedirectToAction("Index", "Posts");
            }
        }
    }
}
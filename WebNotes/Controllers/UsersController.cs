using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNotes.Crypt;
using WebNotesDataBase.DAL;
using WebNotesDataBase.Models;
using WebNotesDataBase.ViewModels;

namespace WebNotes.Controllers
{
    public class UsersController : Controller
    {
        private UserRepository uowUser;
        public GenericRepository<User> userRepository;

        public UsersController()
        {
            uowUser = new UserRepository();
            userRepository = uowUser.unitOfWork.EntityRepository;
        }

        // GET Users
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //POST Cookies User | Delete
        [HttpGet]
        public ActionResult LogOut()
        {
            if(Request.Cookies["login"] != null)
            {
                var cookie = new HttpCookie("login");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("../Users/Login");
        }

        //POST User | Authorization
        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            User auth = uowUser.GetByEmail(EncryptDecrypt.EncryptData(email, pass));
            if(auth != null && auth.Pass == pass && auth.ConfirmEmail == true)
            {
                var cookie = new HttpCookie("login")
                {
                    Value = auth.UserId.ToString(),
                    Expires = DateTime.Now.AddMonths(1)
                };
                Response.SetCookie(cookie);
                return RedirectToAction("../Notes/Index");
            }
            else
            {
                ViewBag.ErrorLogin = "User data not found or not confirmed";
                return View("Login");
            }
        }

        // POST Create Users | Registration
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterUserViewModel model)
        {
            if (ModelState.IsValid && uowUser.GetByEmail(EncryptDecrypt.EncryptData(model.Email, model.Pass)) == null)
            {
                var user = Mapper.Map<RegisterUserViewModel, User>(model);
                user.FirstName = EncryptDecrypt.EncryptData(user.FirstName, user.Pass);
                user.LastName = EncryptDecrypt.EncryptData(user.LastName, user.Pass);
                user.Email = EncryptDecrypt.EncryptData(user.Email, user.Pass);
                user.Birthday = EncryptDecrypt.EncryptData(user.Birthday, user.Pass);
                user.ConfirmEmail = false;
                ////////////////////////////
                MailAddress from = new MailAddress("immortalis82@gmail.com", "CryptoWebNotes Registration");
                MailAddress to = new MailAddress(model.Email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Email confirmation";
                m.Body = string.Format("For registration click on the link:" +
                                "<a href=\"{0}\" title=\"Confirm registration\">{0}</a>",
                    Url.Action("ConfirmEmail", "Users", new { email = model.Email, pass = model.Pass }, Request.Url.Scheme));
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("immortalis82@gmail.com", /*"DANGERZONE"*/);
                smtp.Send(m);
                ///////////////////////////////
                userRepository.Insert(user);
                userRepository.Save();
                ViewBag.ErrorLogin = "Please, confirm your email address";
                return View("Login");
            }
            else
            {
                ViewBag.ErrorRegistration = "This email address is already registered";
                return View("Registration");
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string email, string pass)
        {
            User confirmUser = uowUser.GetByEmail(EncryptDecrypt.EncryptData(email, pass));
            if (confirmUser != null && confirmUser.Pass == pass)
            {
                confirmUser.ConfirmEmail = true;
                userRepository.Update(confirmUser);
                return View("Login");
            }
            else
            {
                ViewBag.ErrorLogin = "Incorrect data";
                return View("Login");
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Get(int? id)
        {
            User loginUser = null;
            if(ModelState.IsValid)
            {
                loginUser = userRepository.GetByID(id);
                loginUser.FirstName = EncryptDecrypt.DecryptData(loginUser.FirstName, loginUser.Pass);
                loginUser.LastName = EncryptDecrypt.DecryptData(loginUser.LastName, loginUser.Pass);
                loginUser.Email = EncryptDecrypt.DecryptData(loginUser.Email, loginUser.Pass);
                loginUser.Birthday = EncryptDecrypt.DecryptData(loginUser.Birthday, loginUser.Pass);
                if (loginUser.About != null && loginUser.About != "")
                {
                    loginUser.About = EncryptDecrypt.DecryptData(loginUser.About, loginUser.Pass);
                }
                userRepository.Save();
                return View("Index", loginUser);
            }
            return RedirectToAction("Login");
        }

        //GET Users Details
        public ActionResult Details()
        {
            RegisterUserViewModel user = null;
            if (Request.Cookies["login"] != null)
            {
                int id = Convert.ToInt32(Request.Cookies["login"].Value);
                user = Mapper.Map<User, RegisterUserViewModel>(userRepository.GetByID(id));
                user.FirstName = EncryptDecrypt.DecryptData(user.FirstName, user.Pass);
                user.LastName = EncryptDecrypt.DecryptData(user.LastName, user.Pass);
                user.Email = EncryptDecrypt.DecryptData(user.Email, user.Pass);
                user.Birthday = EncryptDecrypt.DecryptData(user.Birthday, user.Pass);
                if (user.About != null && user.About != "")
                {
                    user.About = EncryptDecrypt.DecryptData(user.About, user.Pass);
                }
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET Users Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = Mapper.Map<User, EditUserViewModel>(userRepository.GetByID(id));
            user.FirstName = EncryptDecrypt.DecryptData(user.FirstName, user.Pass);
            user.LastName = EncryptDecrypt.DecryptData(user.LastName, user.Pass);
            user.Birthday = EncryptDecrypt.DecryptData(user.Birthday, user.Pass);
            if (user.About != null && user.About != "")
            {
                user.About = EncryptDecrypt.DecryptData(user.About, user.Pass);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST Users Edit
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            User user = Mapper.Map<EditUserViewModel, User>(model);
            User usr = userRepository.GetByID(user.UserId);
            usr.FirstName = EncryptDecrypt.EncryptData(user.FirstName, usr.Pass);
            usr.LastName = EncryptDecrypt.EncryptData(user.LastName, usr.Pass);
            usr.Birthday = EncryptDecrypt.EncryptData(user.Birthday, usr.Pass);
            if (user.About != null && user.About != "")
            {
                usr.About = EncryptDecrypt.EncryptData(user.About, usr.Pass);
            }
            userRepository.Update(usr);
            userRepository.Save();
            return RedirectToAction("../Notes/Index");
        }
    }
}

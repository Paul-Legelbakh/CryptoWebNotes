using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

        // GET: Users
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

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

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            User auth = uowUser.GetByEmail(EncryptDecrypt.EncryptData(email, pass));
            if(auth != null && auth.Pass == pass)
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
                ViewBag.ErrorLogin = auth == null ? "Not registered" : "Wrong password";
                return View("Login");
            }
        }

        // POST: Users/Create
        //registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterUserViewModel model)
        {
            if (ModelState.IsValid && uowUser.GetByEmail(EncryptDecrypt.EncryptData(model.Email, model.Pass)) == null)
            {
                var user = Mapper.Map<RegisterUserViewModel, User>(model);
                user.NameAuthor = EncryptDecrypt.EncryptData(user.NameAuthor, user.Pass);
                user.Email = EncryptDecrypt.EncryptData(user.Email, user.Pass);
                userRepository.Insert(user);
                userRepository.Save();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.ErrorRegistration = "This email is already registered";
                return View("Registration");
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
                loginUser.NameAuthor = EncryptDecrypt.DecryptData(loginUser.NameAuthor, loginUser.Pass);
                loginUser.Email = EncryptDecrypt.DecryptData(loginUser.Email, loginUser.Pass);
                userRepository.Save();
                return View("Index", loginUser);
            }
            return RedirectToAction("Login");
        }

        //GET: Users/Details/5
        public ActionResult Details()
        {
            RegisterUserViewModel user = null;
            if (Request.Cookies["login"] != null)
            {
                int id = Convert.ToInt32(Request.Cookies["login"].Value);
                user = Mapper.Map<User, RegisterUserViewModel>(userRepository.GetByID(id));
                user.NameAuthor = EncryptDecrypt.DecryptData(user.NameAuthor, user.Pass);
                user.Email = EncryptDecrypt.DecryptData(user.Email, user.Pass);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = Mapper.Map<User, EditUserViewModel>(userRepository.GetByID(id));
            user.NameAuthor = EncryptDecrypt.DecryptData(user.NameAuthor, user.Pass);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<EditUserViewModel, User>(model);
                User usr = userRepository.GetByID(user.UserId);
                usr.NameAuthor = EncryptDecrypt.EncryptData(user.NameAuthor, user.Pass);
                usr.Birthday = user.Birthday;
                usr.Email = EncryptDecrypt.EncryptData(user.Email, user.Pass);
                usr.Pass = user.Pass;
                userRepository.Update(usr);
                userRepository.Save();
            }
            return View(model);
        }
    }
}

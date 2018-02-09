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
            User auth = uowUser.GetByEmail(EncryptData(email, pass));
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
            if (ModelState.IsValid && uowUser.GetByEmail(EncryptData(model.Email, model.Pass)) == null)
            {
                var user = Mapper.Map<RegisterUserViewModel, User>(model);
                user.NameAuthor = EncryptData(user.NameAuthor, user.Pass);
                user.Email = EncryptData(user.Email, user.Pass);
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
                loginUser.NameAuthor = DecryptData(loginUser.NameAuthor, loginUser.Pass);
                loginUser.Email = DecryptData(loginUser.Email, loginUser.Pass);
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
                user.NameAuthor = DecryptData(user.NameAuthor, user.Pass);
                user.Email = DecryptData(user.Email, user.Pass);
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
            user.NameAuthor = DecryptData(user.NameAuthor, user.Pass);
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
                usr.NameAuthor = EncryptData(user.NameAuthor, user.Pass);
                usr.Birthday = user.Birthday;
                usr.Email = EncryptData(user.Email, user.Pass);
                usr.Pass = user.Pass;
                userRepository.Update(usr);
                userRepository.Save();
            }
            return View(model);
        }

        private static string EncryptPassword(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private string EncryptData(string textData, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC; //set the mode for operation of the algorithm  
            objrij.Padding = PaddingMode.PKCS7; //set the padding mode used in the algorithm.
            objrij.KeySize = 256; //set the size, in bits, for the secret key. 
            objrij.BlockSize = 256; //set the block size in bits for the cryptographic operation. 
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptPassword(Encryptionkey, "northernviewtechnologies")); //set the symmetric key that is used for encryption & decryption.  
            //set the initialization vector (IV) for the symmetric algorithm    
            byte[] EncryptionkeyBytes = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length) len = EncryptionkeyBytes.Length;
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            ICryptoTransform objtransform = objrij.CreateEncryptor(); //Creates symmetric AES object with the current key and initialization vector IV. 
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length)); //Final transform the test string. 
        }

        private string DecryptData(string EncryptedText, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;
            objrij.KeySize = 256;
            objrij.BlockSize = 256;
            byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptPassword(Encryptionkey, "northernviewtechnologies"));
            byte[] EncryptionkeyBytes = new byte[0x20];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length) len = EncryptionkeyBytes.Length;
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);  //it will return readable string  
        }
    }
}

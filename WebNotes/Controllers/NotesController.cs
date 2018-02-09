using AutoMapper;
using PagedList;
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
    public class NotesController : Controller
    {
        //create new connections of database
        // GET: Notes
        private UserRepository uowUser;
        private NoteRepository uowNote;
        public GenericRepository<Note> noteRepository;
        public GenericRepository<User> userRepository;

        public NotesController()
        {
            uowUser = new UserRepository();
            uowNote = new NoteRepository();
            noteRepository = uowNote.unitOfWork.EntityRepository;
            userRepository = uowUser.unitOfWork.EntityRepository;
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (Request.Cookies["login"] != null)
            {
                var notes = Mapper.Map<IEnumerable<Note>, List<IndexNoteViewModel>>(uowNote.GetListByUserID(Convert.ToInt32(Request.Cookies["login"].Value)));
                return View(notes.ToPagedList(pageNumber, pageSize));
            }
            else return RedirectToAction("../Users/Login");
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var note = Mapper.Map<Note, IndexNoteViewModel>(noteRepository.GetByID(id));
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                User usr = userRepository.GetByID(Convert.ToInt64(Request.Cookies["login"].Value));
                var note = Mapper.Map<CreateNoteViewModel, Note>(model);
                note.CreatedDate = DateTime.Now;
                note.EditedDate = DateTime.Now;
                note.UserId = usr.UserId;
                note.Label = EncryptData(note.Label, usr.Pass);
                note.Body = EncryptData(note.Body, usr.Pass);
                noteRepository.Insert(note);
                noteRepository.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var note = Mapper.Map<Note, CreateNoteViewModel>(noteRepository.GetByID(id));
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateNoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Note note = Mapper.Map<CreateNoteViewModel, Note>(model);
                Note nt = noteRepository.GetByID(note.NoteId);
                nt.EditedDate = DateTime.Now;
                nt.Label = note.Label;
                nt.Body = note.Body;
                noteRepository.Update(nt);
                noteRepository.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var note = Mapper.Map<Note, IndexNoteViewModel>(noteRepository.GetByID(id));
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            noteRepository.Delete(id);
            noteRepository.Save();
            return RedirectToAction("Index");
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
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptPassword(Encryptionkey, "northernviewtech")); //set the symmetric key that is used for encryption & decryption.  
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
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptPassword(Encryptionkey, "northernviewtech"));
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

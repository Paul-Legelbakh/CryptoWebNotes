using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using WebNotesDataBase.Models;
using AutoMapper;
using WebNotesDataBase.ViewModels;

namespace WebNotes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Note, IndexNoteViewModel>();
                    cfg.CreateMap<CreateNoteViewModel, Note>();
                    cfg.CreateMap<Note, CreateNoteViewModel>();
                    cfg.CreateMap<User, EditUserViewModel>();
                    cfg.CreateMap<EditUserViewModel, User>();
                    cfg.CreateMap<RegisterUserViewModel, User>();
                    cfg.CreateMap<User, RegisterUserViewModel>();
                    cfg.CreateMap<LoginUserViewModel, User>();
                    cfg.CreateMap<User, LoginUserViewModel>();
                });
            }
            catch { }
        }
    }
}

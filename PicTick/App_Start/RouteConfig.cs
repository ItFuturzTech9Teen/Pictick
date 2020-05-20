using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PicTick
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Account Controller
            routes.MapRoute(name: "AdminLogin", url: "AdminLogin", defaults: new { Controller = "Account", Action = "AdminLogin" });
            routes.MapRoute(name: "StudioLogin", url: "StudioLogin", defaults: new { Controller = "Account", Action = "StudioLogin" });

            //Home Controller
            routes.MapRoute(name: "Dashboard", url: "Dashboard", defaults: new { Controller = "Home", Action = "Dashboard" });
            routes.MapRoute(name: "Studio", url: "Studio", defaults: new { Controller = "Home", Action = "Studio" });
            routes.MapRoute(name: "StudioAbout", url: "StudioAbout", defaults: new { Controller = "Home", Action = "StudioAbout" });
            routes.MapRoute(name: "Gallery", url: "Gallery", defaults: new { Controller = "Home", Action = "Gallery" });
            routes.MapRoute(name: "Album", url: "Album/{id}", defaults: new { Controller = "Home", Action = "Album", id = UrlParameter.Optional });
            routes.MapRoute(name: "AlbumPhoto", url: "AlbumPhoto", defaults: new { Controller = "Home", Action = "AlbumPhoto" });
            routes.MapRoute(name: "AlbumVideo", url: "AlbumVideo", defaults: new { Controller = "Home", Action = "AlbumVideo" });
            routes.MapRoute(name: "Customer", url: "Customer", defaults: new { Controller = "Home", Action = "Customer" });
            routes.MapRoute(name: "SortListedAlbum", url: "SortListedAlbum", defaults: new { Controller = "Home", Action = "CustomerPhotoSelection" });
            routes.MapRoute(name: "Portfolio", url: "Portfolio", defaults: new { Controller = "Home", Action = "Portfolio" });
            routes.MapRoute(name: "Organization", url: "Organization", defaults: new { Controller = "Home", Action = "Organization" });
            routes.MapRoute(name: "State", url: "State", defaults: new { Controller = "Home", Action = "State" });
            routes.MapRoute(name: "City", url: "City", defaults: new { Controller = "Home", Action = "City" });
            routes.MapRoute(name: "AppointmentSlot", url: "AppointmentSlot", defaults: new { Controller = "Home", Action = "AppointmentSlot" });
            routes.MapRoute(name: "Appointment", url: "Appointment", defaults: new { Controller = "Home", Action = "Appointment" });
            routes.MapRoute(name: "CustomerList", url: "CustomerList", defaults: new { Controller = "Home", Action = "CustomerList" });

            //Client Controller
            routes.MapRoute(name: "ClientGallery", url: "ClientGallery/{id}", defaults: new { Controller = "Client", Action = "Gallery", id = UrlParameter.Optional });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Dashboard", id = UrlParameter.Optional }
            );
        }
    }
}

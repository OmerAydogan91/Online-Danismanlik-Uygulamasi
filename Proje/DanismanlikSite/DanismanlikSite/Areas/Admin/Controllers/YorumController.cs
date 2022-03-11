using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DanismanlikSite.Models;

namespace DanismanlikSite.Areas.Admin.Controllers
{
    public class YorumController : Controller
    {
        // GET: Admin/Yorum
        DB_DanismanlikEntities db = new DB_DanismanlikEntities();
       

        // Admin panelinde onay bekleyen yorumları listeler.
        [HttpGet]
        public ActionResult OnayBekleyenYorum()
        {
            var yorumlar = db.tbl_Yorum.Where(y => y.OnaylandiMi == false).ToList();
            return View(yorumlar);
        }


        // Admin panelinde onaylı yorumları listeler.
        public ActionResult OnayliYorumlar()
        {
            var yorumlar = db.tbl_Yorum.Where(y => y.OnaylandiMi == true).ToList();
            return View(yorumlar);
        }

        // Onay bekleyen yorumları onaylamayı sağlayan method.
        public ActionResult Onayla(int id)
        {
            tbl_Yorum yorum = db.tbl_Yorum.Where(y => y.id_Yorum == id).SingleOrDefault();
            yorum.OnaylandiMi = true;
          
            int basarili = db.SaveChanges();
            if (basarili > 0)
            {
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return View();
        }


        // Yorumları siler.
        public ActionResult YorumSil(int id)
        {
            tbl_Yorum yorum = db.tbl_Yorum.FirstOrDefault(p => p.id_Yorum == id);
            db.tbl_Yorum.Remove(yorum);
            int basarili = db.SaveChanges();

            if (basarili > 0)
            {
                return RedirectToAction("OnayliYorumlar");
            }
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DanismanlikSite.Models;

namespace DanismanlikSite.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        public ActionResult Index()
        {
            return View();
        }

        // Kullanıcın siteye giriş yapmasını sağlayan method.
        public ActionResult GirisYap()
        {
            //kullanıcının oturum acma islemi basarili ise
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Kullanici = User.Identity.Name;
                return RedirectToAction("Index", "Anasayfa");
            }
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(tbl_Kullanici k)
        {
            //validation control
            if (ModelState.IsValid)
            {
                using (DB_DanismanlikEntities ctx = new DB_DanismanlikEntities())
                {
                    var kullanici = ctx.tbl_Kullanici.FirstOrDefault(x => x.Eposta == k.Eposta && x.Sifre == k.Sifre);
                    if (kullanici != null)
                    {
                        FormsAuthentication.SetAuthCookie(kullanici.AdSoyad, true);
                        FormsAuthentication.SetAuthCookie(kullanici.id_Kullanici.ToString(), true);
                        return RedirectToAction("Index", "Anasayfa");
                    }
                    else
                    {
                        ViewBag.Uyari = "Kullanici Adı ya da Şifre yanlış!";
                    }
                }
            }
            return View();
        }


        // Siteden çıkış yapar.
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Anasayfa");
        }


        // Siteye üye olma
        public ActionResult UyeOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeOl(tbl_Kullanici k, string sampleradio)
        {

            using (DB_DanismanlikEntities ctx = new DB_DanismanlikEntities())
            {
                k.id_KullaniciRol = 2;
                ctx.tbl_Kullanici.Add(k);
                int basarili  = ctx.SaveChanges();

                if (basarili > 0)
                {
                    return RedirectToAction("Index","Anasayfa");

                }
                return View();
            }

        }
    }
}
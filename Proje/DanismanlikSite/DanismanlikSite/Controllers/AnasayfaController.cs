using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DanismanlikSite.Areas.Admin.Data;
using DanismanlikSite.Models;

namespace DanismanlikSite.Controllers
{
    public class AnasayfaController : Controller
    {
        // GET: Anasayfa
        DB_DanismanlikEntities db = new DB_DanismanlikEntities();
     

        // Anasayfada projeleri son eklenene göre listeler.
        public ActionResult Index()
        {
            return View(db.tbl_Proje.ToList().OrderByDescending(t => t.id_Proje));
          
        } 

        // Kategoriye göre Projeleri listeler.
        public ActionResult KategoriyeGoreProjeler(int id)
        {
            List<tbl_Proje> projeler = db.tbl_Proje.Where(p => p.id_Kategori == id).ToList();
            ViewBag.Kategori = db.tbl_Kategori.Where(k => k.id_Kategori == id).First().KategoriAdi;
            return View(projeler);
        }

        // Projelerin detay sayfası.Detay sayfasında proje açıklamalarına, indirme linkine, yorum kısmına ve fotoğraf galerisine erişiyoruz.
        public ActionResult ProjeDetay(int id)
        {
            tbl_Proje proje = db.tbl_Proje.Where(p=>p.id_Proje == id).SingleOrDefault();
            return View(proje);
        }


        // Projelerin altında yorum yapmayı sağlayan method.
        [HttpPost]
        public ActionResult YorumYap(tbl_Yorum y)
        {      
            y.YorumTarihi = DateTime.Now;
            y.OnaylandiMi = false;

            db.tbl_Yorum.Add(y);
            int basarili = db.SaveChanges();
            
            if (basarili > 0)
            {
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return View();
        }


        //Site içinde proje aramayı sağlar. Proje adına ya da kategoriye göre arama yapılabilir.
        [HttpGet]
        public ActionResult SiteIciAra(string ProjeAra)
        {
            ViewBag.Search = ProjeAra;
            List<tbl_Proje> projeler = db.tbl_Proje.
            Where(t => t.ProjeAdi.ToLower().Contains(ProjeAra.ToLower()) || t.tbl_Kategori.KategoriAdi.ToLower().Contains(ProjeAra.ToLower())).ToList();
            ViewBag.Proje = projeler;
            return View(ViewBag.Proje);
        }


        // Sitede oturum açan kullanıcı isterse proje ekleyebilir.
        [HttpGet]
        public ActionResult ProjeEkle()
        {
            ProjeViewModel pwm = new ProjeViewModel();
            List<SelectListItem> KategoriListesi = new List<SelectListItem>();


            foreach (tbl_Kategori item in db.tbl_Kategori.ToList())
            {
                KategoriListesi.Add(new SelectListItem { Value = item.id_Kategori.ToString(), Text = item.KategoriAdi });
            }

            pwm.KategoriListesi = KategoriListesi;
            pwm.Proje = null;
            return View(pwm);
        }

    }
}
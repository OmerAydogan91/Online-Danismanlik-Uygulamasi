using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DanismanlikSite.Models;

namespace DanismanlikSite.Areas.Admin.Controllers
{
    public class KategoriController : Controller
    {
        // GET: Admin/Kategori

        DB_DanismanlikEntities db = new DB_DanismanlikEntities();

        // Admin panelinde kategorileri listeler.
        public ActionResult Listele()
        {
            return View(db.tbl_Kategori.ToList());
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }

        // Admin panelinde yeni kategori eklemeyi sağlar.
        [HttpPost]
        public ActionResult KategoriEkle(tbl_Kategori model)
        {
            db.tbl_Kategori.Add(model);
            int basarili = db.SaveChanges();
            if (basarili > 0)
            {
                return RedirectToAction("Listele");
            }
            return View(model);
        }

        // Var olan kategori üzerinde güncelleme işlemi yapmayı sağlar.
        [HttpGet]
        public ActionResult KategoriGuncelle(int id)
        {
            var kategori = db.tbl_Kategori.Where(t => t.id_Kategori == id).FirstOrDefault();
            return View(kategori);
        }

        [HttpPost]
        public ActionResult KategoriGuncelle(tbl_Kategori model)
        {

            tbl_Kategori guncelle = db.tbl_Kategori.SingleOrDefault(t => t.id_Kategori == model.id_Kategori);
            guncelle.KategoriAdi = model.KategoriAdi;
            guncelle.Aciklama = model.Aciklama;

            int basarili = db.SaveChanges();
            if (basarili > 0)
            {
                return RedirectToAction("Listele");
            }
            return View(model);
        }

        // Kategorileri siler.
        public ActionResult KategoriSil(int id)
        {

            tbl_Kategori kategori = db.tbl_Kategori.FirstOrDefault(p => p.id_Kategori == id);
            db.tbl_Kategori.Remove(kategori);
            int basarili = db.SaveChanges();

            if (basarili > 0)
            {
                return RedirectToAction("Listele");
            }
            return View();
        }

    }
}
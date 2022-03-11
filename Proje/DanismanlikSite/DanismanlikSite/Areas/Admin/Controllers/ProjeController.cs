using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DanismanlikSite.Areas.Admin.Data;
using DanismanlikSite.Models;

namespace DanismanlikSite.Areas.Admin.Controllers
{
    public class ProjeController : Controller
    {
        // GET: Admin/Proje

        DB_DanismanlikEntities db = new DB_DanismanlikEntities();

        // Admin panelinde projeleri listeler.
        public ActionResult Listele()
        {
            return View(db.tbl_Proje.ToList());
        }


        // Yeni proje eklemeyi sağlayan method.
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

        [HttpPost]
        public ActionResult ProjeEkle(ProjeViewModel model, IEnumerable<HttpPostedFileBase> files, HttpPostedFileBase ekdosya)
        {
            var fileName = "";
            int success = 0;

            db.tbl_Proje.Add(model.Proje);
            int projeKayit = db.SaveChanges();

            foreach (var file in files)
            {
                tbl_ProjeFoto pf = new tbl_ProjeFoto();

                if (file != null)
                {
                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file.SaveAs(path);


                    pf.id_Proje = model.Proje.id_Proje;
                    pf.FotoURL = fileName;
                    db.tbl_ProjeFoto.Add(pf);
                    success = db.SaveChanges();
                }

            }

            try
            {
                tbl_EkDosya dosya = new tbl_EkDosya();
                var dosyaAdi = "";

                dosyaAdi = Guid.NewGuid().ToString().Replace("-", "") + Path.GetFileName(ekdosya.FileName);

                dosya.id_Proje = model.Proje.id_Proje;
                dosya.EkDosyaAd = ekdosya.FileName;

                var path = Path.Combine(Server.MapPath("~/Dosyalar"), dosyaAdi);
                ekdosya.SaveAs(path);
               
                dosya.EkDosyaURL = dosyaAdi;

                db.tbl_EkDosya.Add(dosya);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            if (projeKayit > 0 || success > 0)
            {
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return View(model);
        }



        //Projeyi güncellemeyi sağlayan method.
        [HttpGet]
        public ActionResult ProjeGuncelle(int id)
        {
            ProjeViewModel pwm = new ProjeViewModel();
            List<SelectListItem> KategoriListesi = new List<SelectListItem>();


            foreach (tbl_Kategori item in db.tbl_Kategori.ToList())
            {
                KategoriListesi.Add(new SelectListItem { Value = item.id_Kategori.ToString(), Text = item.KategoriAdi });
            }

            pwm.KategoriListesi = KategoriListesi;
            pwm.Proje = db.tbl_Proje.Where(t => t.id_Proje == id).FirstOrDefault();
            return View(pwm);
        }

        [HttpPost]
        public ActionResult ProjeGuncelle(ProjeViewModel model)
        {
            tbl_Proje guncelle = db.tbl_Proje.SingleOrDefault(t => t.id_Proje == model.Proje.id_Proje);
            guncelle.ProjeAdi = model.Proje.ProjeAdi;
            guncelle.Aciklama = model.Proje.Aciklama;
            guncelle.id_Kategori = model.Proje.id_Kategori;

            int basarili = db.SaveChanges();

            if (basarili > 0)
            {
                return RedirectToAction("Listele");
            }
            return View(model);
        }


        // Projeyi siler.
        public ActionResult ProjeSil(int id)
        {

            tbl_Proje proje = db.tbl_Proje.FirstOrDefault(p => p.id_Proje == id);
            db.tbl_Proje.Remove(proje);
            int basarili = db.SaveChanges();

            if (basarili > 0)
            {
                return RedirectToAction("Listele");
            }
            return View();
        }


        // Projeyi ait fotoğrafları listeler.
        public ActionResult FotografListesi()
        {
            var foto = db.tbl_ProjeFoto.ToList();
            return View(foto);
        }
    }
}
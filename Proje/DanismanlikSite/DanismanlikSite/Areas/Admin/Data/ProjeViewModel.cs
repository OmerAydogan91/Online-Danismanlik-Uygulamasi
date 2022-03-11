using DanismanlikSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DanismanlikSite.Areas.Admin.Data
{
    public class ProjeViewModel
    {
        public tbl_Proje Proje { get; set; }
        public IEnumerable<SelectListItem> KategoriListesi { get; set; }
       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace technosoru.ViewModel
{
    public class YanitlarModel
    {
        public int Yanitid { get; set; }
        public string YanitIcerik { get; set; }
        public System.DateTime YanitTarih { get; set; }
        public int yUyeid { get; set; }
        public int ySoruid { get; set; }
        public string ySoruBaslik { get; set; }
        public int SoruYanitSayi { get; set; }
    }
}
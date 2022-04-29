using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace technosoru.ViewModel
{
    public class SorularModel
    {
        public int Soruid { get; set; }
        public string SoruBaslik { get; set; }
        public string SoruIcerik { get; set; }
        public System.DateTime SoruTarih { get; set; }
        public string SoruKatAd { get; set; }
        public int sUyeid { get; set; }
        public int sKatid { get; set; }
    }
}
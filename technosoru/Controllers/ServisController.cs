using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using technosoru.Models;
using technosoru.ViewModel;


namespace technosoru.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();
        #region Kategoriler 
        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategorilerModel> KategoriListe()
        {
            List<KategorilerModel> liste = db.Kategoriler.Select(x => new KategorilerModel()
            {
                Katid = x.Katid,
                KatAd = x.KatAd,
                katSoruSayi = x.Sorular.Count()
            }).ToList();
            return liste;
        }



        [HttpGet]
        [Route("api/kategoribyid/{Katid}")]
        public KategorilerModel KategoriById(int Katid)
        {
            KategorilerModel kayit = db.Kategoriler.Where(s => s.Katid == Katid).Select(x => new KategorilerModel()
            {
                Katid = x.Katid, KatAd = x.KatAd, katSoruSayi = x.Sorular.Count()
            }).FirstOrDefault();
            return kayit; }





        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategorilerModel model)
        {
            if (db.Kategoriler.Count(s => s.KatAd == model.KatAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }
            Kategoriler yeni = new Kategoriler();
            yeni.KatAd = model.KatAd;
            db.Kategoriler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategorilerModel model)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.Katid == model.Katid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.KatAd = model.KatAd;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }


        [HttpDelete]
        [Route("api/kategorisil/{Katid}")]
        public SonucModel KategoriSil(int Katid)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.Katid == Katid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            if (db.Sorular.Count(s => s.sKatid == Katid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Ürün  Kaydı Olan Kategori Silinemez!";
                return sonuc;
            }
            db.Kategoriler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion


        #region Sorular


        [HttpGet]
        [Route("api/soruliste")]
        public List<SorularModel> SoruListe()
        {
            List<SorularModel> liste = db.Sorular.Select(x => new SorularModel()
            {
                Soruid = x.Soruid,
                SoruBaslik = x.SoruBaslik,
                SoruIcerik = x.SoruIcerik,
                SoruTarih = x.SoruTarih,
                sKatid = x.sKatid,
                SoruKatAd = x.Kategoriler.KatAd,
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/sorulistebykatid/{Katid}")]
        public List<SorularModel> SorularListeByKatid(int Katid)
        {
            List<SorularModel> liste = db.Sorular.Where(s => s.sKatid == Katid).Select(x => new SorularModel()
            {
                Soruid = x.Soruid,
                SoruBaslik = x.SoruBaslik,
                SoruIcerik = x.SoruIcerik,
                SoruTarih = x.SoruTarih,
                SoruKatAd = x.Kategoriler.KatAd,
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/sorubyid/{Soruid}")]
        public SorularModel SoruById(int Soruid)
        {
            SorularModel kayit = db.Sorular.Where(s => s.Soruid == Soruid).Select(x => new SorularModel()
            {
                Soruid = x.Soruid,
                SoruBaslik = x.SoruBaslik,
                SoruIcerik = x.SoruIcerik,
                SoruTarih = x.SoruTarih,
                SoruKatAd = x.Kategoriler.KatAd,
            }).FirstOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/soruekle")]
        public SonucModel UrunEkle(SorularModel model)
        {
            if (db.Sorular.Count(s => s.SoruBaslik == model.SoruBaslik && s.sKatid == model.sKatid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Soru İlgili Kategoride Kayıtlıdır!";
                return sonuc;
            }
            Sorular yeni = new Sorular();
            yeni.SoruBaslik = model.SoruBaslik;
            yeni.SoruIcerik = model.SoruIcerik;
            yeni.sKatid = model.sKatid;
            yeni.SoruTarih = model.SoruTarih;
            yeni.sUyeid = model.sUyeid;
            db.Sorular.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/soruduzenle")]
        public SonucModel UrunDuzenle(SorularModel model)
        {
            Sorular kayit = db.Sorular.Where(s => s.Soruid == model.Soruid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.SoruBaslik = model.SoruBaslik;
            kayit.SoruIcerik = model.SoruIcerik;
            kayit.sKatid = model.sKatid;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/sorusil/{Soruid}")]
        public SonucModel SoruSil(int Soruid)
        {
            Sorular kayit = db.Sorular.Where(s => s.Soruid == Soruid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Sorular.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Silindi";
            return sonuc;
        }
        #endregion


        #region Yanitlar

        [HttpGet]
        [Route("api/yanitliste")]
        public List<YanitlarModel> YanitListe()
        {
            List<YanitlarModel> liste = db.Yanitlar.Select(x => new YanitlarModel()
            {
                Yanitid = x.Yanitid,
                YanitIcerik = x.YanitIcerik,
                YanitTarih = x.YanitTarih,
                ySoruid = x.ySoruid,
                ySoruBaslik = x.Sorular.SoruBaslik,
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/yanitbysoruid/{Yanitid}")]
        public YanitlarModel YanitlarBySoruid(int Yanitid)
        {
            YanitlarModel kayit = db.Yanitlar.Where(s => s.Yanitid == Yanitid).Select(x => new YanitlarModel()
            {
                Yanitid = x.Yanitid,
                YanitIcerik = x.YanitIcerik,
                ySoruid = x.ySoruid,
                YanitTarih = x.YanitTarih,
                ySoruBaslik = x.Sorular.SoruBaslik,
                SoruYanitSayi = x.Sorular.Yanitlar.Count()
            }).FirstOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/yanitekle")]
        public SonucModel YanitEkle(YanitlarModel model)
        {
            if (db.Yanitlar.Count(s => s.YanitIcerik == model.YanitIcerik && s.ySoruid == model.ySoruid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Yanıt İlgili Soruda Kayıtlıdır!";
                return sonuc;
            }
            Yanitlar yeni = new Yanitlar();
            yeni.YanitIcerik = model.YanitIcerik;
            yeni.YanitTarih = model.YanitTarih;
            yeni.ySoruid = model.ySoruid;
            yeni.yUyeid = model.yUyeid;
            db.Yanitlar.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yanıt Eklendi";
            return sonuc;
        }


        [HttpPut]
        [Route("api/yanitduzenle")]
        public SonucModel YanitDuzenle(YanitlarModel model)
        {
            Yanitlar kayit = db.Yanitlar.Where(s => s.Yanitid == model.Yanitid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yanıt Bulunamadı!";
                return sonuc;
            }
            kayit.YanitIcerik = model.YanitIcerik;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yanıt Düzenlendi";
            return sonuc;
        }


        [HttpDelete]
        [Route("api/yanitsil/{Yanitid}")]
        public SonucModel YanitSil(int Yanitid)
        {
            Yanitlar kayit = db.Yanitlar.Where(s => s.Yanitid == Yanitid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Yanitlar.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yanıt Silindi";
            return sonuc;
        }

        #endregion


        #region Uyeler
        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyelerModel> UyeListe()
        {
            List<UyelerModel> liste = db.Uyeler.Select(x => new UyelerModel()
            {
                Uyeid = x.Uyeid,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeParola = x.UyeParola,
                UyeYetki = x.UyeYetki
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/uyebyid/{Uyeid}")]
        public UyelerModel UyeByid(int Uyeid)
        {
            UyelerModel kayit = db.Uyeler.Where(s => s.Uyeid == Uyeid).Select(x => new UyelerModel()
            {
                Uyeid = x.Uyeid,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeParola = x.UyeParola,
                UyeYetki = x.UyeYetki
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyelerModel model)
        {
            if (db.Uyeler.Count(s => s.UyeMail == model.UyeMail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen E-Posta Adresi Kayıtlıdır!";
                return sonuc;
            }
            Uyeler yeni = new Uyeler();
            yeni.UyeAdSoyad = model.UyeAdSoyad;
            yeni.UyeMail = model.UyeMail;
            yeni.UyeParola = model.UyeParola;
            yeni.UyeTarih = model.UyeTarih;
            yeni.UyeYetki = model.UyeYetki;
            db.Uyeler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyelerModel model)
        {
            Uyeler kayit = db.Uyeler.Where(s => s.Uyeid == model.Uyeid).SingleOrDefault();
            if (kayit == null) {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            kayit.UyeAdSoyad = model.UyeAdSoyad;
            kayit.UyeMail = model.UyeMail;
            kayit.UyeParola = model.UyeParola;
            kayit.UyeYetki = model.UyeYetki;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{Uyeid}")]
        public SonucModel UyeSil(int Uyeid)
        {
            Uyeler kayit = db.Uyeler.Where(s => s.Uyeid == Uyeid).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            if (db.Sorular.Count(s => s.Soruid == Uyeid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Soru Kaydı Olan Üye Silinemez!";
                return sonuc;
            }
            if (db.Yanitlar.Count(s => s.Yanitid == Uyeid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Yanıt Kaydı Olan Üye Silinemez!";
                return sonuc;
            }
            db.Uyeler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }
        #endregion
    }
}

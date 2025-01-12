using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace OgrenciDersYonetimSistemi
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public abstract void BilgiGoster();
    }

    public interface ILogin
    {
        void Login(string username, string password);
    }

    public class Ogrenci : Person, ILogin
    {
        public string OgrenciNo { get; set; }

        public override void BilgiGoster()
        {
            Console.WriteLine($"Öğrenci: {Name}, No: {OgrenciNo}, Email: {Email}");
        }

        public void Login(string username, string password)
        {
            Console.WriteLine($"{Name} kullanıcısı giriş yaptı.");
        }
    }

    public class Ogretmen : Person, ILogin
    {
        public override void BilgiGoster()
        {
            Console.WriteLine($"Öğretmen: {Name}, Email: {Email}");
        }

        public void Login(string username, string password)
        {
            Console.WriteLine($"{Name} öğretmen giriş yaptı.");
        }
    }

    public class Ders
    {
        public string DersAdi { get; set; }
        public int Kredi { get; set; }
        public Ogretmen Ogretmen { get; set; }
        public List<Ogrenci> KayitliOgrenciler { get; set; } = new List<Ogrenci>();

        public void DersBilgileriGoster()
        {
            Console.WriteLine($"Ders: {DersAdi}, Kredi: {Kredi}, Öğretmen: {Ogretmen.Name}");
            Console.WriteLine("Kayıtlı Öğrenciler:");
            foreach (var ogrenci in KayitliOgrenciler)
            {
                Console.WriteLine($"- {ogrenci.Name} ({ogrenci.OgrenciNo})");
            }
        }

        public void OgrenciKaydet(Ogrenci ogrenci)
        {
            if (!KayitliOgrenciler.Contains(ogrenci))
            {
                KayitliOgrenciler.Add(ogrenci);
                Console.WriteLine($"{ogrenci.Name} derse kaydedildi.");
            }
            else
            {
                Console.WriteLine($"{ogrenci.Name} zaten bu derse kayıtlı.");
            }
        }
    }

    class Program
    {
        static List<Ders> dersler = new List<Ders>();
        static string derslerDosya = "dersler.json";
        static string ogrencilerDosya = "ogrenciler.json";
        static string ogretmenlerDosya = "ogretmenler.json";

        static void Main(string[] args)
        {
            Console.WriteLine("--- Öğrenci ve Ders Yönetim Sistemi ---");
            Yukle();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Öğretim Görevlisi Tanımla");
                Console.WriteLine("2. Ders Tanımla");
                Console.WriteLine("3. Öğrenci Tanımla");
                Console.WriteLine("4. Öğrenciyi Derse Kaydet");
                Console.WriteLine("5. Ders Bilgilerini Görüntüle");
                Console.WriteLine("6. Verileri Kaydet ve Çıkış");
                Console.Write("Seçiminizi yapınız: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        OgretimGorevlisiEkle();
                        break;
                    case "2":
                        DersEkle();
                        break;
                    case "3":
                        OgrenciEkle();
                        break;
                    case "4":
                        OgrenciyiDerseKaydet();
                        break;
                    case "5":
                        DersleriGoruntule();
                        break;
                    case "6":
                        Kaydet();
                        Console.WriteLine("Veriler başarıyla kaydedildi. Çıkış yapılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen 1 ile 6 arasında geçerli bir seçenek girin.");
                        break;
                }

                Console.WriteLine("Devam etmek için bir tuşa basın...");
                Console.ReadKey();
            }
        }

        static void OgretimGorevlisiEkle()
        {
            var ogretmenler = File.Exists(ogretmenlerDosya)
                ? JsonConvert.DeserializeObject<List<Ogretmen>>(File.ReadAllText(ogretmenlerDosya)) ?? new List<Ogretmen>()
                : new List<Ogretmen>();

            Console.Write("Ad: ");
            string ad = Console.ReadLine();
            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            ogretmenler.Add(new Ogretmen
            {
                Id = ogretmenler.Count + 1,
                Name = $"{ad} {soyad}",
                Email = email
            });

            File.WriteAllText(ogretmenlerDosya, JsonConvert.SerializeObject(ogretmenler, Formatting.Indented));
            Console.WriteLine("Öğretim Görevlisi başarıyla tanımlandı!");
        }

        static void DersEkle()
        {
            var ogretmenler = JsonConvert.DeserializeObject<List<Ogretmen>>(File.ReadAllText(ogretmenlerDosya)) ?? new List<Ogretmen>();
            if (ogretmenler.Count == 0)
            {
                Console.WriteLine("Önce bir öğretim görevlisi tanımlamanız gerekiyor!");
                return;
            }

            Console.Write("Ders Adı: ");
            string dersAdi = Console.ReadLine();
            Console.Write("Ders Kredisi: ");
            if (!int.TryParse(Console.ReadLine(), out int kredi) || kredi <= 0)
            {
                Console.WriteLine("Geçersiz kredi değeri!");
                return;
            }

            Console.WriteLine("Öğretmen Seçiniz:");
            for (int i = 0; i < ogretmenler.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ogretmenler[i].Name}");
            }

            if (!int.TryParse(Console.ReadLine(), out int ogretmenSecim) || ogretmenSecim < 1 || ogretmenSecim > ogretmenler.Count)
            {
                Console.WriteLine("Geçersiz seçim!");
                return;
            }

            var ders = new Ders
            {
                DersAdi = dersAdi,
                Kredi = kredi,
                Ogretmen = ogretmenler[ogretmenSecim - 1]
            };

            dersler.Add(ders);
            Kaydet();
            Console.WriteLine("Ders başarıyla tanımlandı!");
        }

        static void OgrenciEkle()
        {
            var ogrenciler = File.Exists(ogrencilerDosya)
                ? JsonConvert.DeserializeObject<List<Ogrenci>>(File.ReadAllText(ogrencilerDosya)) ?? new List<Ogrenci>()
                : new List<Ogrenci>();

            Console.Write("Ad: ");
            string ad = Console.ReadLine();
            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();
            Console.Write("Öğrenci Numarası: ");
            string ogrenciNo = Console.ReadLine();

            ogrenciler.Add(new Ogrenci
            {
                Id = ogrenciler.Count + 1,
                Name = $"{ad} {soyad}",
                OgrenciNo = ogrenciNo
            });

            File.WriteAllText(ogrencilerDosya, JsonConvert.SerializeObject(ogrenciler, Formatting.Indented));
            Console.WriteLine("Öğrenci başarıyla tanımlandı!");
        }

        static void OgrenciyiDerseKaydet()
        {
            if (dersler.Count == 0)
            {
                Console.WriteLine("Henüz ders tanımlanmamış!");
                return;
            }

            Console.WriteLine("Ders seçiniz:");
            for (int i = 0; i < dersler.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dersler[i].DersAdi}");
            }

            if (!int.TryParse(Console.ReadLine(), out int dersIndex) || dersIndex < 1 || dersIndex > dersler.Count)
            {
                Console.WriteLine("Geçersiz seçim!");
                return;
            }

            var ogrenciler = JsonConvert.DeserializeObject<List<Ogrenci>>(File.ReadAllText(ogrencilerDosya)) ?? new List<Ogrenci>();

            if (ogrenciler.Count == 0)
            {
                Console.WriteLine("Henüz öğrenci tanımlanmamış!");
                return;
            }

            Console.WriteLine("Öğrenci seçiniz:");
            for (int i = 0; i < ogrenciler.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ogrenciler[i].Name}");
            }

            if (!int.TryParse(Console.ReadLine(), out int ogrenciIndex) || ogrenciIndex < 1 || ogrenciIndex > ogrenciler.Count)
            {
                Console.WriteLine("Geçersiz seçim!");
                return;
            }

            dersler[dersIndex - 1].OgrenciKaydet(ogrenciler[ogrenciIndex - 1]);
            Kaydet();
        }

        static void DersleriGoruntule()
        {
            if (dersler.Count > 0)
            {
                foreach (var ders in dersler)
                {
                    ders.DersBilgileriGoster();
                }
            }
            else
            {
                Console.WriteLine("Henüz bir ders tanımlanmadı!");
            }
        }

        static void Kaydet()
        {
            try
            {
                File.WriteAllText(derslerDosya, JsonConvert.SerializeObject(dersler, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kaydetme sırasında bir hata oluştu: {ex.Message}");
            }
        }

        static void Yukle()
        {
            try
            {
                if (File.Exists(derslerDosya))
                {
                    dersler = JsonConvert.DeserializeObject<List<Ders>>(File.ReadAllText(derslerDosya)) ?? new List<Ders>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yükleme sırasında bir hata oluştu: {ex.Message}");
                dersler = new List<Ders>();
            }
        }
    }
}

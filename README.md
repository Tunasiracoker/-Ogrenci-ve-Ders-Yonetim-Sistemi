# Öğrenci ve Ders Yönetim Sistemi

Bu proje, öğrencilerin derslere kaydedilmesini, ders bilgilerini yönetmeyi ve listelemeyi sağlayan bir **C# Console Uygulamasıdır**. Proje, nesne yönelimli programlama (OOP) prensiplerini kullanarak temel bir öğrenci-ders yönetim sistemi oluşturmayı amaçlar.

## Özellikler

### Ders Yönetimi:
1. **Yeni Ders Eklenebilir:**
   - Her ders için ders adı, kredisi ve öğretmeni tanımlanabilir.
   - Derslere kayıtlı öğrenciler listelenebilir.

2. **Öğrenci Yönetimi:**
   - Derslere öğrenci kaydı yapılabilir.
   - Her öğrenci için ad-soyad, e-posta ve öğrenci numarası girilebilir.

3. **Listeleme:**
   - Tüm dersler ve kayıtlı öğrenciler detaylı şekilde görüntülenebilir.

4. **Kullanıcı Girişi:**
   - Öğretmenler ve öğrenciler `ILogin` arayüzü ile sisteme giriş yapabilir.

---

## Projenin Teknik Yapısı

### Sınıflar (Classes):
1. **`Person` (Temel Sınıf):**
   - Ortak özellikler: `Id`, `Name`
   - Ortak davranış: `DisplayInfo()` (abstract metot)

2. **`Ogrenci` (Öğrenci Sınıfı):**
   - `Person` sınıfından türetilmiştir.
   - Ek özellik: `Department`
   - JSON dosyasıyla veri kaydetme ve yükleme işlemleri.

3. **`OgretimGorevlisi` (Öğretim Görevlisi Sınıfı):**
   - `Person` sınıfından türetilmiştir.
   - Ek özellik: `Title`
   - JSON dosyasıyla veri kaydetme ve yükleme işlemleri.

4. **`Ders` (Ders Sınıfı):**
   - Ders bilgilerini tutar: `Name`, `Credits`, `Instructor`, `Students`.
   - JSON dosyasıyla veri kaydetme ve yükleme işlemleri.

### Arayüz (Interface):
- **`ILogin` Arayüzü:**
  - Kullanıcı giriş işlemleri için bir yapı sağlar.
  - Öğretmen ve öğrenci sınıfları tarafından uygulanır.

---

## Uygulama Akışı

1. **Öğrenci Oluşturma:**
   - Öğrenci bilgileri girilir ve JSON dosyasına kaydedilir.

2. **Öğretim Görevlisi Oluşturma:**
   - Öğretim görevlisi bilgileri girilir ve JSON dosyasına kaydedilir.

3. **Ders Oluşturma:**
   - Yeni ders eklenir, öğretim görevlisi atanır ve öğrenci kaydedilir.

4. **Veri Görüntüleme:**
   - JSON dosyalarından yüklenen bilgiler detaylı olarak ekrana yazdırılır:
     - Öğrenci bilgileri
     - Öğretim görevlisi bilgileri
     - Ders bilgileri ve kayıtlı öğrenciler

---

## Örnek Çıktı
```plaintext
Ogrenci: Ali, Id: 1, Department: Bilgisayar Muhendisligi
OgretimGorevlisi: Dr. Ahmet, Id: 101, Title: Doçent
Ders: Programlama, Credits: 3, Instructor: Dr. Ahmet
Students:
- Ali

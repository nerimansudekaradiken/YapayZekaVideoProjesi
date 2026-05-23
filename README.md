# 🎬 Yapay Zeka Entegrasyonlu Video Üretme Projesi

Bu proje, C# Windows Forms kullanılarak geliştirilmiş, tek bir metin girdisiyle (prompt) uçtan uca otomatik olarak hikaye yazan, görsel üreten, seslendirme yapan ve tüm bunları akıllı altyazılarla birleştirerek bir MP4 videosu çıkaran tam kapsamlı bir masaüstü uygulamasıdır.

## ✨ Temel Özellikler

* **🧠 Dinamik Hikaye Üretimi (Google Gemini API):** Kullanıcının girdiği kısa bir konuyu baz alarak, 4 paragraflık akıcı bir hikaye ve her bir paragraf için özel İngilizce görsel üretim komutları (prompt) oluşturur.
* **🎨 Otomatik Görsel Oluşturma (Pollinations AI):** Gemini tarafından üretilen İngilizce komutları işleyerek her sahne için 1920x1080 çözünürlüğünde, yüksek detaylı ve sinematik görseller oluşturur. (Çapa kuralı ile sahneler arası tutarlılık sağlanır).
* **🗣️ Metin Okuma (TTS - Text to Speech):** Üretilen Türkçe hikayeyi sahne sahne otomatik olarak seslendirir ve WAV formatında kaydeder.
* **⏱️ Akıllı Altyazı Senkronizasyonu (.SRT):** Ses dosyasının uzunluğunu milisaniye cinsinden hesaplar, metni cümlelere böler ve her cümlenin uzunluğuna göre ekranda kalma süresini otomatik olarak ayarlayarak profesyonel bir SRT altyazı dosyası oluşturur.
* **🎥 Video Render & Montaj (FFmpeg):** Görselleri, sesleri ve altyazıları birleştirir. Görsellere dinamik "Zoom & Pan" (Ken Burns) efekti ekleyerek MP4 formatında sahneler oluşturur ve son aşamada tüm sahneleri birleştirerek `Final_Hikaye.mp4` çıktısını verir.
* **🗄️ Veritabanı Kaydı (MS SQL Server):** Üretilen temiz hikayeleri ve kullanıcı girdilerini loglamak amacıyla SQL Server veritabanına kaydeder.

## 🛠️ Kullanılan Teknolojiler

* **Dil & Platform:** C#, .NET 8.0, Windows Forms
* **Veritabanı:** MS SQL Server (ADO.NET)
* **Dış API'ler:** Google Gemini 2.5 Flash API, Pollinations AI API
* **Araçlar:** FFmpeg, System.Speech.Synthesis, Newtonsoft.Json

---

## 🚀 Kurulum ve Çalıştırma Rehberi

Projeyi yerel bilgisayarınızda (Localhost) çalıştırmak için aşağıdaki adımları izleyin:

### 1. Veritabanı Kurulumu
1. SQL Server Management Studio (SSMS) programını açın.
2. Proje dizinindeki `Database` klasöründe bulunan `AIVideoProjectDB.sql` dosyasını çalıştırarak `AIVideoProjectDB` veritabanını ve `Stories` tablosunu oluşturun.
3. `Form1.cs` dosyası içerisindeki `connectionString` değişkenini kendi yerel SQL Server adınıza (örn: `Server=localhost;...`) göre güncelleyin.

### 2. FFmpeg Entegrasyonu
Proje, videoları işlemek için FFmpeg aracını kullanmaktadır.
* Test kolaylığı sağlamak amacıyla gerekli olan **`ffmpeg.exe`** dosyası proje ana dizinine dahil edilmiştir. Ekstra bir indirme yapmanıza gerek yoktur. Proje Visual Studio'da derlendiğinde, bu dosyanın `bin/Debug/net8.0-windows` klasörü içinde olduğundan emin olunması yeterlidir.

### 3. API Anahtarları
Projenin metin üretim aşaması Google Gemini API kullanmaktadır. `Form1.cs` içerisinde bulunan `apiKey` değişkenine kendi Google Gemini API anahtarınızı giriniz.

---

## 💻 Kullanım

1. Projeyi Visual Studio üzerinden başlatın (`F5`).
2. Üst kısımdaki metin kutusuna oluşturmak istediğiniz videonun konusunu yazın (Örn: *Karlar altında sihirli fenerlerle aydınlatılmış bir köy*).
3. **"Hikaye Üret"** butonuna basın. Sol taraftaki log ekranından API isteklerini, ayrıştırma (parsing) işlemlerini ve görsel/ses üretim süreçlerini canlı olarak takip edebilirsiniz.
4. Tüm sahnelerin görselleri arayüze yüklendikten sonra **"Video Üret"** butonuna basın.
5. FFmpeg işlemi tamamlandığında, projenin derleme klasöründe `Final_Hikaye.mp4` adlı profesyonel videonuz hazır olacaktır!

## ⚙️ Sistem Mimarisi & Hata Yönetimi
Proje, asenkron (`async/await`) yapıda tasarlanmıştır, bu sayede ağır API istekleri veya FFmpeg video işleme süreçlerinde arayüz (UI) donmaz. API kaynaklı yoğunluk (Örn: *HTTP 503 Service Unavailable*) veya JSON ayıklama sırasındaki "büyük-küçük harf duyarlılığı" gibi senaryolar için özel hata ayıklayıcılar (parsers) ve `try-catch` blokları entegre edilmiştir.
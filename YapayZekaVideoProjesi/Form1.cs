using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Speech.Synthesis;

namespace YapayZekaVideoProjesi
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = @"Server=.;Database=AIVideoProjectDB;Trusted_Connection=True;";
        private readonly string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "AIzaSyAwSjPh_GAJN6K9wTr3ltlXIt8t0dzaBGc";

        private static readonly HttpClient sharedClient = new HttpClient();

        private string sonUretilenHikaye = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {
            // Tasarımcı hatası almamak için burası durmalı
        }

        private string DosyaYoluAl(string dosyaAdi) => Path.Combine(Application.StartupPath, dosyaAdi);

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            string prompt = txtPrompt.Text;

            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Lütfen bir konu girin!");
                btnGenerate.Enabled = true;
                return;
            }

            rtbLog.Clear();
            rtbLog.AppendText("🚀 İşlem başlatıldı...\n");

            EskiResimleriSil();

            try
            {
                // AŞAMA 1: Gemini'dan Ham Veriyi Al
                string geminiYanit = await HikayeUretLLM(prompt);

                if (geminiYanit.StartsWith("API Hatası"))
                {
                    rtbLog.AppendText($"❌ GOOGLE'IN YANITI: {geminiYanit}\n");
                    btnGenerate.Enabled = true;
                    return;
                }

                // AŞAMA 2: KESİN GARANTİLİ AYIKLAMA (PARSING)
                rtbLog.AppendText("📝 Metin ve Görsel komutları ayrıştırılıyor...\n");

                List<string> hikayeParcalari = new List<string>();
                List<string> resimPromptlari = new List<string>();

                string temizYanit = geminiYanit.Replace("**", "");
                string[] satirlar = temizYanit.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                string aktifTur = "";

                foreach (string satir in satirlar)
                {
                    string temizSatir = satir.Trim();
                    if (string.IsNullOrWhiteSpace(temizSatir)) continue;

                    string buyukSatir = temizSatir.ToUpper(new System.Globalization.CultureInfo("en-US"));

                    if (buyukSatir.StartsWith("PARAGRAF") || buyukSatir.StartsWith("SAHNE"))
                    {
                        aktifTur = "PARAGRAF";
                        int index = temizSatir.IndexOf(":");
                        string icerik = (index != -1) ? temizSatir.Substring(index + 1).Trim() : temizSatir;
                        hikayeParcalari.Add(icerik);
                    }
                    else if (buyukSatir.StartsWith("PROMPT") || buyukSatir.StartsWith("GÖRSEL"))
                    {
                        aktifTur = "PROMPT";
                        int index = temizSatir.IndexOf(":");
                        string icerik = (index != -1) ? temizSatir.Substring(index + 1).Trim() : temizSatir;
                        resimPromptlari.Add(icerik);
                    }
                    else
                    {
                        if (aktifTur == "PARAGRAF" && hikayeParcalari.Count > 0)
                            hikayeParcalari[hikayeParcalari.Count - 1] += " " + temizSatir;
                        else if (aktifTur == "PROMPT" && resimPromptlari.Count > 0)
                            resimPromptlari[resimPromptlari.Count - 1] += " " + temizSatir;
                    }
                }

                if (hikayeParcalari.Count < 4 || resimPromptlari.Count < 4)
                {
                    rtbLog.AppendText($"⚠️ Uyarı: Sahneler eksik üretildi! (Hikaye: {hikayeParcalari.Count}, Prompt: {resimPromptlari.Count})\n");
                    btnGenerate.Enabled = true;
                    return;
                }

                sonUretilenHikaye = string.Join("\n", hikayeParcalari);

                rtbLog.AppendText("\n📜 --- ÜRETİLEN HİKAYE ---\n");
                rtbLog.AppendText(sonUretilenHikaye + "\n");
                rtbLog.AppendText("---------------------------\n\n");

                VeritabaninaKaydet(prompt, sonUretilenHikaye);
                rtbLog.AppendText("✔️ Hikaye SQL'e kaydedildi.\n");

                // AŞAMA 3: SES VE GÖRSEL ÜRETİMİ
                rtbLog.AppendText("🎬 Sahneler hazırlanıyor...\n");
                int hikayeTohumu = new Random().Next(1, 99999);

                for (int i = 0; i < 4; i++)
                {
                    int sahneNo = i + 1;
                    rtbLog.AppendText($"🔄 Sahne {sahneNo} işleniyor...\n");

                    await SesUret(hikayeParcalari[i], DosyaYoluAl($"ses{sahneNo}.wav"));
                    await GorselUret(resimPromptlari[i], (PictureBox)this.Controls.Find($"picSahne{sahneNo}", true)[0], $"Sahne{sahneNo}", hikayeTohumu);

                    rtbLog.AppendText($"✔️ Sahne {sahneNo} tamam.\n");
                }

                rtbLog.AppendText("🎉 Tüm storyboard ve sesler hazır! Videoyu oluşturabilirsiniz.\n");
            }
            catch (Exception ex)
            {
                rtbLog.AppendText("❌ Kritik Hata: " + ex.Message + "\n");
            }
            finally
            {
                btnGenerate.Enabled = true;
            }
        }

        private async Task<string> HikayeUretLLM(string konu)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            string sistemKomutu = "Sen profesyonel bir video senaryo yazarısın. " +
            "Sana verilen konuyu kullanarak 4 paragraflık bir hikaye ve her paragraf için İngilizce görsel komutları (prompt) yaz. " +
            "\n\nFORMAT ŞÖYLE OLMALIDIR:\n" +
            "PARAGRAF 1: [Metin]\n" +
            "PROMPT 1: [İngilizce Görsel Detaylar]\n\n" +
            "⚠️ KRİTİK 'GÖRSEL ÇAPA' KURALI (TUTARLILIK İÇİN):\n" +
            "Hikayenin ana konusu ne olursa olsun (insanlar, eşyalar, manzaralar veya soyut bir yapay zeka), 1. PROMPT'ta belirlediğin 'Ana Nesne'yi, 'Mekanı' ve 'Atmosferi' BİREBİR KOPYALAYARAK 2, 3 ve 4. PROMPT'ların EN BAŞINA yapıştırmalısın.\n" +
            "Örnek İnsanlı Çapa: 'Three girls in a cozy cafe, rainy window, amber lighting...'\n" +
            "Örnek İnsansız Çapa: 'A futuristic AI server room, glowing blue cables, dark cyberpunk atmosphere...'\n" +
            "Bu 'çapa' metni her promptta aynı kalmalı, sadece sonuna o sahnenin ufak detayı (close up of a coffee cup, wide shot of the server, vb.) eklenmelidir.\n\n" +
            "⚠️ DİĞER KURALLAR:\n" +
            "1. Toplam metin 200-240 kelime olmalı.\n" +
            "2. PROMPT kısımları tamamen İngilizce ve somut olmalı.\n" +
            "3. Sadece istenen formatta cevap ver, açıklama yapma.\n\n" +
            "Konu: " + konu;

            var body = new { contents = new[] { new { parts = new[] { new { text = sistemKomutu } } } } };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await sharedClient.PostAsync(url, content);
            string resStr = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic json = JsonConvert.DeserializeObject(resStr);
                return json.candidates[0].content.parts[0].text;
            }
            return "API Hatası: " + resStr;
        }

        private async Task GorselUret(string ingilizcePrompt, PictureBox box, string ad, int seed)
        {
            string yol = DosyaYoluAl(ad + ".jpg");
            try
            {
                if (box.Image != null) { box.Image.Dispose(); box.Image = null; }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                string tamPrompt = $"{ingilizcePrompt}, realistic, cinematic, 8k, highly detailed";
                string url = $"https://image.pollinations.ai/prompt/{Uri.EscapeDataString(tamPrompt)}?width=1920&height=1080&seed={seed}";

                var bytes = await sharedClient.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(yol, bytes);

                using (var ms = new MemoryStream(bytes))
                {
                    box.Image = new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                rtbLog.AppendText($"❌ Görsel Hatası ({ad}): {ex.Message}\n");
            }
        }

        public async Task SesUret(string metin, string yol)
        {
            await Task.Run(() => {
                using (var synth = new SpeechSynthesizer())
                {
                    foreach (var voice in synth.GetInstalledVoices())
                    {
                        if (voice.VoiceInfo.Culture.TwoLetterISOLanguageName == "tr")
                        {
                            synth.SelectVoice(voice.VoiceInfo.Name);
                            break;
                        }
                    }
                    synth.SetOutputToWaveFile(yol);
                    synth.Speak(metin);
                }
            });
        }

        private async void btnVideoUret_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sonUretilenHikaye))
            {
                MessageBox.Show("Lütfen önce bir hikaye ve görsel üretin!");
                return;
            }

            btnVideoUret.Enabled = false;
            rtbLog.AppendText("\n🚀 VİDEO RENDER BAŞLIYOR...\n");

            try
            {
                string ffmpegYolu = DosyaYoluAl("ffmpeg.exe");
                var p = sonUretilenHikaye.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i <= 4; i++)
                {
                    string resimYolu = DosyaYoluAl($"Sahne{i}.jpg");
                    string sesYolu = DosyaYoluAl($"ses{i}.wav");
                    string parcaVideoYolu = DosyaYoluAl($"sahne{i}.mp4");
                    string srtYolu = DosyaYoluAl($"sahne{i}.srt");

                    double toplamSureMs = SesSuresiniHesapla(sesYolu) * 1000;

                    AltyaziSrtOlustur(p[i - 1], srtYolu, toplamSureMs);

                    string arguman = $"-y -r 25 -loop 1 -i \"{resimYolu}\" -i \"{sesYolu}\" " +
                                     $"-vf \"scale=2560:1440,zoompan=z='1.10+0.0006*on':x='(iw-iw/zoom)/2':y='(ih-ih/zoom)/2':d=375:s=1920x1080:fps=25,fade=t=in:st=0:d=1,subtitles='sahne{i}.srt'\" " +
                                     $"-c:v libx264 -preset ultrafast -crf 23 -pix_fmt yuv420p " +
                                     $"-c:a aac -shortest \"{parcaVideoYolu}\"";

                    await FFmpegCalistir(ffmpegYolu, arguman);
                    rtbLog.AppendText($"✔️ Sahne {i} MP4 ve Cümle Altyazıları hazır.\n");
                }

                string listeYolu = DosyaYoluAl("liste.txt");
                File.WriteAllText(listeYolu, "file 'sahne1.mp4'\nfile 'sahne2.mp4'\nfile 'sahne3.mp4'\nfile 'sahne4.mp4'");

                string finalVideo = DosyaYoluAl("Final_Hikaye.mp4");
                string concatArg = $"-y -f concat -safe 0 -i \"{listeYolu}\" -c copy \"{finalVideo}\"";

                await FFmpegCalistir(ffmpegYolu, concatArg);
                rtbLog.AppendText("🎥 BİTTİ! Final_Hikaye.mp4 oluşturuldu.\n");

                // Videoyu bilgisayarın varsayılan oynatıcısında otomatik olarak açar
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = finalVideo,
                    UseShellExecute = true
                });
            }
            catch (Exception ex) { rtbLog.AppendText("❌ Video Hatası: " + ex.Message + "\n"); }
            finally { btnVideoUret.Enabled = true; }
        }

        private void AltyaziSrtOlustur(string paragraf, string srtYolu, double toplamSureMs)
        {
            string srtIcerik = "";
            int srtSira = 1;
            double suankiZamanMs = 0;

            string[] cumleler = paragraf.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int toplamKarakter = paragraf.Length;

            foreach (string cumle in cumleler)
            {
                string temizCumle = cumle.Trim();
                if (string.IsNullOrWhiteSpace(temizCumle)) continue;

                temizCumle += ".";

                double oran = (double)temizCumle.Length / (double)toplamKarakter;
                double cumleSuresiMs = toplamSureMs * oran;

                TimeSpan baslangic = TimeSpan.FromMilliseconds(suankiZamanMs);
                suankiZamanMs += cumleSuresiMs;
                TimeSpan bitis = TimeSpan.FromMilliseconds(suankiZamanMs);

                srtIcerik += $"{srtSira}\r\n";
                srtIcerik += $"{baslangic:hh\\:mm\\:ss\\,fff} --> {bitis:hh\\:mm\\:ss\\,fff}\r\n";
                srtIcerik += $"{temizCumle}\r\n\r\n";

                srtSira++;
            }

            File.WriteAllText(srtYolu, srtIcerik, new System.Text.UTF8Encoding(false));
        }

        private async Task FFmpegCalistir(string ffmpegYolu, string argumanlar)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffmpegYolu,
                Arguments = argumanlar,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Application.StartupPath
            };
            using (var process = System.Diagnostics.Process.Start(psi))
            {
                await Task.Run(() => process.WaitForExit());
            }
        }

        private void VeritabaninaKaydet(string pr, string hik)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Stories (UserPrompt, StoryContent, ImagePath1, ImagePath2, ImagePath3, ImagePath4) VALUES (@p, @c, @i1, @i2, @i3, @i4)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@p", pr);
                    cmd.Parameters.AddWithValue("@c", hik);
                    cmd.Parameters.AddWithValue("@i1", "Sahne1.jpg");
                    cmd.Parameters.AddWithValue("@i2", "Sahne2.jpg");
                    cmd.Parameters.AddWithValue("@i3", "Sahne3.jpg");
                    cmd.Parameters.AddWithValue("@i4", "Sahne4.jpg");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { MessageBox.Show("SQL Hatası: " + ex.Message); }
        }

        private void EskiResimleriSil()
        {
            picSahne1.Image = null; picSahne2.Image = null; picSahne3.Image = null; picSahne4.Image = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 1; i <= 4; i++)
            {
                string p = DosyaYoluAl($"Sahne{i}.jpg");
                if (File.Exists(p)) try { File.Delete(p); } catch { }
            }
        }

        private double SesSuresiniHesapla(string wavYolu)
        {
            try
            {
                byte[] header = new byte[32];
                using (FileStream fs = new FileStream(wavYolu, FileMode.Open, FileAccess.Read))
                {
                    fs.Read(header, 0, 32);
                }
                int byteRate = BitConverter.ToInt32(header, 28);
                long fileSize = new FileInfo(wavYolu).Length;

                return (double)(fileSize - 44) / byteRate;
            }
            catch { return 15.0; }
        }
    }
}
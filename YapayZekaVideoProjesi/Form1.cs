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
        string connectionString = @"Server=.;Database=AIVideoProjectDB;Trusted_Connection=True;";
        string apiKey = "BURAYA_API_GELECEK";

        string sonUretilenHikaye = ""; // Sadece temiz hikaye metnini tutacak

        public Form1()
        {
            InitializeComponent();
        }
        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {
            // Tasarımcı hatası almamak için burası durmalı
        }



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

                // AŞAMA 2: AYIKLAMA (PARSING)
                rtbLog.AppendText("📝 Metin ve Görsel komutları ayrıştırılıyor...\n");

                string[] satirlar = geminiYanit.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> hikayeParcalari = new List<string>();
                List<string> resimPromptlari = new List<string>();

                foreach (var satir in satirlar)
                {
                    if (satir.StartsWith("PARAGRAF"))
                        hikayeParcalari.Add(satir.Substring(satir.IndexOf(":") + 1).Trim());
                    else if (satir.StartsWith("PROMPT"))
                        resimPromptlari.Add(satir.Substring(satir.IndexOf(":") + 1).Trim());
                }

                // Güvenlik kontrolü
                if (hikayeParcalari.Count < 4 || resimPromptlari.Count < 4)
                {
                    rtbLog.AppendText("⚠️ Uyarı: Bazı sahneler eksik üretildi!\n");
                    btnGenerate.Enabled = true; return;
                }

                // Video butonunun okuyacağı 'temiz' hikayeyi oluştur (Etiketler olmadan)
                sonUretilenHikaye = string.Join("\n", hikayeParcalari);

                rtbLog.AppendText("\n📜 --- ÜRETİLEN HİKAYE ---\n");
                rtbLog.AppendText(sonUretilenHikaye + "\n");
                rtbLog.AppendText("---------------------------\n\n");

                // Veritabanına temiz hikayeyi kaydet
                VeritabaninaKaydet(prompt, sonUretilenHikaye);
                rtbLog.AppendText("✔️ Hikaye SQL'e kaydedildi.\n");

                // AŞAMA 3: SES VE GÖRSEL ÜRETİMİ
                rtbLog.AppendText("🎬 Sahneler hazırlanıyor...\n");

                int hikayeTohumu = new Random().Next(1, 99999);

                for (int i = 0; i < 4; i++)
                {
                    int sahneNo = i + 1;
                    rtbLog.AppendText($"🔄 Sahne {sahneNo} işleniyor...\n");

                    // Ses ve Görseli üret (Sırayla)
                    await SesUret(hikayeParcalari[i], Path.Combine(Application.StartupPath, $"ses{sahneNo}.wav"));
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
            using (HttpClient client = new HttpClient())
            {
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

                var response = await client.PostAsync(url, content);
                string resStr = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic json = JsonConvert.DeserializeObject(resStr);
                    return json.candidates[0].content.parts[0].text;
                }
                return "API Hatası: " + resStr;
            }
        }

        private async Task GorselUret(string ingilizcePrompt, PictureBox box, string ad, int seed)
        {
            string yol = Path.Combine(Application.StartupPath, ad + ".jpg");
            try
            {
                if (box.Image != null) { box.Image.Dispose(); box.Image = null; }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                string tamPrompt = $"{ingilizcePrompt}, realistic, cinematic, 8k, highly detailed";
                string url = $"https://image.pollinations.ai/prompt/{Uri.EscapeDataString(tamPrompt)}?width=1920&height=1080&seed={seed}";
                using (HttpClient client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(yol, bytes);
                    using (var ms = new MemoryStream(bytes))
                    {
                        box.Image = new Bitmap(ms);
                    }
                }
            }
            catch (Exception ex) { rtbLog.AppendText($"❌ Görsel Hatası ({ad}): {ex.Message}\n"); }
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
                string ffmpegYolu = Path.Combine(Application.StartupPath, "ffmpeg.exe");
                var p = sonUretilenHikaye.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i <= 4; i++)
                {
                    string resimYolu = Path.Combine(Application.StartupPath, $"Sahne{i}.jpg");
                    string sesYolu = Path.Combine(Application.StartupPath, $"ses{i}.wav");
                    string parcaVideoYolu = Path.Combine(Application.StartupPath, $"sahne{i}.mp4");

                    string arguman = $"-y -r 25 -loop 1 -i \"{resimYolu}\" -i \"{sesYolu}\" " +
                                     $"-vf \"scale=2560:1440,zoompan=z='1.05+0.0005*on':x='(on/375)*(iw-iw/zoom)':y='(on/375)*(ih-ih/zoom)':d=375:s=1920x1080:fps=25,fade=t=in:st=0:d=1\" " +
                                     $"-c:v libx264 -preset ultrafast -crf 23 -pix_fmt yuv420p " +
                                     $"-c:a aac -shortest \"{parcaVideoYolu}\"";

                    await FFmpegCalistir(ffmpegYolu, arguman);
                    rtbLog.AppendText($"✔️ Sahne {i} MP4 hazır.\n");
                }

                string listeYolu = Path.Combine(Application.StartupPath, "liste.txt");
                File.WriteAllText(listeYolu, "file 'sahne1.mp4'\nfile 'sahne2.mp4'\nfile 'sahne3.mp4'\nfile 'sahne4.mp4'");

                string finalVideo = Path.Combine(Application.StartupPath, "Final_Hikaye.mp4");
                string concatArg = $"-y -f concat -safe 0 -i \"{listeYolu}\" -c copy \"{finalVideo}\"";

                await FFmpegCalistir(ffmpegYolu, concatArg);
                rtbLog.AppendText("🎥 BİTTİ! Final_Hikaye.mp4 oluşturuldu.\n");
            }
            catch (Exception ex) { rtbLog.AppendText("❌ Video Hatası: " + ex.Message + "\n"); }
            finally { btnVideoUret.Enabled = true; }
        }

        private async Task FFmpegCalistir(string ffmpegYolu, string argumanlar)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffmpegYolu,
                Arguments = argumanlar,
                UseShellExecute = false,
                CreateNoWindow = true
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
                string p = Path.Combine(Application.StartupPath, $"Sahne{i}.jpg");
                if (File.Exists(p)) try { File.Delete(p); } catch { }
            }
        }
    }
}
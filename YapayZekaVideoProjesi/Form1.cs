using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YapayZekaVideoProjesi
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=.;Database=AIVideoProjectDB;Trusted_Connection=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void txtPrompt_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text;

            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Lütfen önce bir hikaye konusu girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rtbLog.AppendText("LLM'e istek gönderiliyor...\n");
            btnGenerate.Enabled = false;

            try
            {
                string uretilenHikaye = await HikayeUretLLM(prompt);
                VeritabaninaKaydet(prompt, uretilenHikaye);

                rtbLog.AppendText("Hikaye başarıyla üretildi!\n");
                rtbLog.AppendText("Hikaye başarıyla üretildi!\n");
                rtbLog.AppendText("----------------------------------\n");
                rtbLog.AppendText(uretilenHikaye + "\n");
                rtbLog.AppendText("----------------------------------\n");
            }
            catch (Exception ex)
            {
                rtbLog.AppendText("Bir hata oluştu: " + ex.Message + "\n");
            }
            finally
            {
                btnGenerate.Enabled = true;
            }

        }

        private async Task<string> HikayeUretLLM(string kullaniciKonusu)
        {
            string apiKey = "Buraya_Kendi_API'nizi_Yazın.";
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[] {
                new { parts = new[] { new { text = "Sen profesyonel bir hikaye yazarısın. Lütfen şu konu hakkında kısa ve etkileyici 3 paragraflık bir hikaye yaz: " + kullaniciKonusu } } }
            }
                };

                string jsonPayload = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic json = JsonConvert.DeserializeObject(responseString);
                        string hikaye = json.candidates[0].content.parts[0].text;
                        return hikaye;
                    }
                    else
                    {
                        return "API Hatası: " + responseString;
                    }
                }
                catch (Exception ex)
                {
                    return "Bağlantı Hatası: " + ex.Message;
                }
            }
        }
        private void VeritabaninaKaydet(string prompt, string hikaye)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Stories (UserPrompt, StoryContent) VALUES (@prompt, @content)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@prompt", prompt);
                cmd.Parameters.AddWithValue("@content", hikaye);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    rtbLog.AppendText("✔️ Hikaye veritabanına kalıcı olarak kaydedildi.\n");
                }
                catch (Exception ex)
                {
                    rtbLog.AppendText("❌ Veritabanı hatası: " + ex.Message + "\n");
                }
            }
        }
    }
}
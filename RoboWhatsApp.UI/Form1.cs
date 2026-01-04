using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoboWhatsApp.UI
{
    public partial class Form1 : Form
    {
        OpenQA.Selenium.Chrome.ChromeDriver driver = null;
        SeleniumHelper seleniumHelper;

        public Form1()
        {
            InitializeComponent();
            List<DTO.Contato> lista = new DAL.Contato().getContato(1).GetAwaiter().GetResult().data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenQA.Selenium.Chrome.ChromeDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver())
            {
                this.driver = driver;
                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                seleniumHelper = new SeleniumHelper(driver);

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);

                    //var tags = driver.FindElementsByTagName("img");

                    if (seleniumHelper.hasInnerHtml("To use WhatsApp on your computer:"))
                    {
                        TelaDeLogin();
                    }

                    if (seleniumHelper.hasInnerHtml("data-icon=\"menu\"") && seleniumHelper.waitForInnerHtml("</body>"))
                    {
                        TelaPrincipal();
                    }
                }
            }
        }

        private void TelaDeLogin()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);


                if (seleniumHelper.hasInnerHtml("data-icon=\"menu\""))
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(100);
                        if (seleniumHelper.waitForInnerHtml("</body>"))
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void TelaPrincipal()
        {
            //var x = driver.FindElementsByXPath("//div[(@class= '_3j7s9')]//div[(@class='_1VfKB')]");
            //System.Diagnostics.Debug.WriteLine(DateTime.Now +  " Qtd. Components: " + x.Count);

            //checkStatus();

             List<DTO.Contato> lista = new DAL.Contato().getContato(1).GetAwaiter().GetResult().data;

            foreach (var item in lstCheck)
            {
                string contato = item.Substring(item.IndexOf("_1wjpf\">") + 8, item.IndexOf("<", item.IndexOf("_1wjpf\">")) - item.IndexOf("_1wjpf\">") - 8);
                string status = item.Substring(item.IndexOf("data-icon=\"") + 11, item.IndexOf("\"", item.IndexOf("data-icon=\"") + 11) - item.IndexOf("data-icon=\"")-11);
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Contato: " + contato + " Status: " + status);
            }
        }

        List<string> lstCheck = new List<string>();

        private void checkStatus()
        {
            lstCheck.Clear();

            string style = driver.FindElementByClassName("RLfQR").GetAttribute("style");
            int height = int.Parse(style.Substring(style.IndexOf("height: ") + 8, style.IndexOf("px", style.IndexOf("height: ")) - (style.IndexOf("height: ") + 8)));

            for (int i = 0; i < height; i+=250)
            {
                //document.getElementsByClassName("_1NrpZ")[0].scrollTo(0, 1000)
                driver.ExecuteScript("document.getElementsByClassName(\"_1NrpZ\")[0].scrollTo(0, " + i + ")", null);
                //System.Threading.Thread.Sleep(500);
                var x = driver.FindElementsByClassName("_3j7s9");

                foreach (var item in x)
                {
                    if (item.GetAttribute("innerHTML").Contains("_1VfKB")) { 
                        if (!lstCheck.Contains(item.GetAttribute("innerHTML")))
                            lstCheck.Add(item.GetAttribute("innerHTML"));
                    }
                }
                
            }
        }
    }
}

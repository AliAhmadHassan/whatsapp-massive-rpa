using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RoboWhatsApp.Console
{
    public class SendWhatsMessage
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        private const uint WM_SETTEXT = 0x0C;

        ChromeDriver driver;
        SeleniumHelper seleniumHelper;
        DAL.Contato contatoService;

        public SendWhatsMessage(ChromeDriver driver)
        {
            this.driver = driver;
            seleniumHelper = new SeleniumHelper(this.driver);
            contatoService = new DAL.Contato();
        }

        public void Send(DTO.Contato contato, DTO.Mensagem mensagem)
        {
            IWebElement input = driver.FindElementByXPath("//input[@title='Procurar ou começar uma nova conversa']");
            input.Click();
            input.SendKeys("+" + contato.ddi + contato.ddd + contato.numeroTelefone);
            input.SendKeys("\n");

            if (!string.IsNullOrEmpty(mensagem.texto))
            {
                IWebElement inputMessage = driver.FindElementByXPath("//div[@contenteditable='true']");

                inputMessage.Click();
                inputMessage.SendKeys(mensagem.texto);
                inputMessage.SendKeys("\n");
            }

            if (!string.IsNullOrEmpty(mensagem.caminhoArquivo))
            {
                foreach (var item in driver.FindElementsByClassName("rAUz7"))
                {
                    if (item.GetAttribute("innerHTML").Contains("title=\"Anexar\""))
                    {
                        item.Click();
                        break;
                    }
                }

                tentaNovamente:;

                try
                {
                    foreach (var item in driver.FindElementsByClassName("GK4Lv"))
                    {
                        if (item.GetAttribute("innerHTML").Contains("data-icon=\"image\""))
                        {
                            item.Click();

                            System.Threading.Thread.Sleep(200);


                            break;
                        }
                    }
                }
                catch (Exception erro)
                {
                    goto tentaNovamente;
                }
            }

            if (!string.IsNullOrEmpty(mensagem.caminhoArquivo))
            {
                var handle = FindWindow(null, "Open");

                while (handle.ToInt64() < 1)
                    handle = FindWindow(null, "Open");

                SetForegroundWindow(handle);
                System.Threading.Thread.Sleep(200);
                System.Windows.Forms.SendKeys.SendWait(mensagem.caminhoArquivo);
                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            }

            if (!string.IsNullOrEmpty(mensagem.caminhoArquivo))
            {
                tentaNovamente:;
                try
                {

                    driver.FindElementByClassName("_3nfoJ").Click();
                }
                catch (Exception erro)
                {
                    goto tentaNovamente;
                }
            }



            contato.dtEnvio = DateTime.Now;
        }

        public void selectAllContact(List<DTO.Contato> contatos, DTO.Mensagem mensagem)
        {
            driver.FindElementByClassName("_2MSJr").FindElement(OpenQA.Selenium.By.TagName("input")).Click();
            List<DTO.Contato> temWhats = new List<DTO.Contato>();
            for (int i = 0; i < contatos.Count; i++)
            {
                DTO.Contato contato = contatos[i];

               retornaBotao("rAUz7","New chat").Click();
                var inputUser = driver.FindElementByClassName("jN-F5");
                inputUser.SendKeys("+" + contato.ddi + contato.ddd + contato.numeroTelefone);
                //if (driver.FindElementByClassName("_1OnaQ") != null)
                //    continue;
                
                if (driver.PageSource.Contains("_2fq0t")  && driver.FindElementByClassName("_2fq0t").Text.Contains("No contacts found"))
                {
                    continue;
                }
                if (driver.PageSource.Contains("_3WZoe") && (!driver.PageSource.Contains("_25Ooe")) && driver.FindElementByClassName("_3WZoe").Text.Contains("No chats, contacts or messages found"))
                {
                    continue;
                }
                inputUser.SendKeys("\n");

                

                retornaBotao("rAUz7", "Attach").Click();

                retornaBotao("GK4Lv", "data-icon=\"image\"").Click();

                System.Threading.Thread.Sleep(100);

                if (!string.IsNullOrEmpty(mensagem.caminhoArquivo))
                {
                    var handle = FindWindow(null, "Open");

                    while (handle.ToInt64() < 1)
                        handle = FindWindow(null, "Open");

                    SetForegroundWindow(handle);
                    System.Threading.Thread.Sleep(200);
                    System.Windows.Forms.SendKeys.SendWait(mensagem.caminhoArquivo);
                    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                }


                if (!string.IsNullOrEmpty(mensagem.caminhoArquivo))
                {
                    tentaNovamente:;
                    try
                    {

                        driver.FindElementByClassName("_3nfoJ").Click();
                    }
                    catch (Exception erro)
                    {
                        goto tentaNovamente;
                    }
                }


                /*
                driver.FindElementByClassName("_2MSJr").FindElement(OpenQA.Selenium.By.TagName("input")).Clear();

                driver.FindElementByClassName("_2MSJr").FindElement(OpenQA.Selenium.By.TagName("input")).SendKeys("+" + contato.ddi + contato.ddd + contato.numeroTelefone);
                driver.FindElementByClassName("_2MSJr").FindElement(OpenQA.Selenium.By.TagName("input")).SendKeys("\n");



                System.Threading.Thread.Sleep(200);
                if (driver.FindElementByClassName("_2sNbV").GetAttribute("innerHTML").Contains("_1wjpf"))
                {
                    temWhats.Add(contato);
                    System.IO.File.AppendAllText(@"C:\temp\enviado.txt", "+" + contato.ddi + contato.ddd + contato.numeroTelefone + ";" + contato.id + "\r\n");
                }
                */

                temWhats.Add(contato);
                System.Diagnostics.Debug.WriteLine("Selecionado " + temWhats.Count + " de " + i);
                System.Threading.Thread.Sleep(5000);
            }

        }

        private OpenQA.Selenium.IWebElement retornaBotao(string elemento, string texto)
        {
            OpenQA.Selenium.IWebElement retorno = null;

            var newChat = driver.FindElementsByClassName(elemento);
            foreach (var button in newChat)
            {
                if (button.GetAttribute("innerHTML").Contains(texto))
                {
                    retorno = button;
                    
                    break;
                }
            }
            return retorno;
        }
    }
}

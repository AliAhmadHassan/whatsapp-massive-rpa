using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoboWhatsApp.Console
{
    class Program
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        public static System.Drawing.Color GetPixelColor(IntPtr hwnd, int x, int y)
        {
            IntPtr hdc = GetDC(hwnd);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(hwnd, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                            (int)(pixel & 0x0000FF00) >> 8,
                            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        private const uint WM_SETTEXT = 0x0C;

        static OpenQA.Selenium.Chrome.ChromeDriver driver = null;
        static SeleniumHelper seleniumHelper;

        static void Main(string[] args)
        {
            
            var handle = FindWindow(null, "Genymotion - Trial - Custom Phone - 7.0.0 - API 24 - 768x1280 (768x1280, 320dpi) - 192.168.81.105");
            SetCursorPosition(250, 620);
            /*while (true) {
                MousePoint mousePositition = new MousePoint();
                GetCursorPos(out mousePositition);
                Color color = GetPixelColor(handle, mousePositition.Y, mousePositition.X);
                System.Diagnostics.Debug.WriteLine("Posição ("+mousePositition.X+ ", " + mousePositition.Y + ") Cor: "+color.ToString()+"");
                //var handle = FindWindow(null, "Untitled - Notepad");
            }*/
            List<DTO.Contato> lista = new DAL.Contato().loteParaEnvio(1).GetAwaiter().GetResult().data;
            int count = 0;

            //SetForegroundWindow(handle);

            //SetCursorPos(73, 375);
            //LeftClick();
            //System.Threading.Thread.Sleep(700);
            System.Diagnostics.Debug.WriteLine(DateTime.Now);
            SetForegroundWindow(handle);
            for (int i = 400; i < 500; i++)
            {
                DTO.Contato contato = lista[i];

                SetForegroundWindow(handle);
                SetCursorPos(388, 84);
                LeftClick();
                System.Threading.Thread.Sleep(400);
                SetForegroundWindow(handle);
                System.Windows.Forms.SendKeys.SendWait(contato.ddi.ToString() + contato.ddd.ToString() + contato.numeroTelefone.ToString());
                System.Threading.Thread.Sleep(800);
                Color color = GetPixelColor(handle, 44, 137);

                if(!((color.A.Equals(255) && color.R.Equals(255) && color.G.Equals(255) && color.B.Equals(255))
                    || (color.A.Equals(255) && color.R.Equals(244) && color.G.Equals(244) && color.B.Equals(244)))){
                    System.Threading.Thread.Sleep(200);
                    SetCursorPos(44, 167);
                    LeftClick();
                    System.Threading.Thread.Sleep(400);

                    count++;
                    System.Diagnostics.Debug.WriteLine(i + " -> Color: " + color.ToString() + " Encontrado ("+ count + ")");
                    
                } else
                {
                    System.Diagnostics.Debug.WriteLine(i + " -> Color: " + color.ToString() + " Não Encontrado");
                }

                

                //291 -> Color: Color [A=255, R=255, G=255, B=255]
                //292 -> Color: Color[A = 255, R = 244, G = 244, B = 244]


                if(count > 20)
                    System.Diagnostics.Debug.WriteLine(DateTime.Now);
            }
            System.Diagnostics.Debug.WriteLine(DateTime.Now);
            //System.Windows.Forms.SendKeys.SendWait("{ENTER}");

            return;
            

            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
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

        public static void LeftClick()
        {
            LeftClick(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);
        }

        public static void LeftClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetCursorPosition(MousePoint point)
        {
            SetCursorPos(point.X, point.Y);
        }
        public static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)value,
                 position.X,
                 position.Y,
                 0,
                 0)
                ;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private static void TelaDeLogin()
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

        private static void TelaPrincipal()
        {
            //var x = driver.FindElementsByXPath("//div[(@class= '_3j7s9')]//div[(@class='_1VfKB')]");
            //System.Diagnostics.Debug.WriteLine(DateTime.Now +  " Qtd. Components: " + x.Count);

            //checkStatus();

            List<DTO.Contato> lista = new DAL.Contato().loteParaEnvio(1).GetAwaiter().GetResult().data;
            DTO.Mensagem mensagem = new DAL.Mensagem().findById(1).GetAwaiter().GetResult().data;
            SendWhatsMessage sendMessage = new SendWhatsMessage(driver);
            
            sendMessage.selectAllContact(lista, mensagem);

            var newChat = driver.FindElementsByClassName("rAUz7");
            foreach (var button in newChat)
            {
                if(button.GetAttribute("innerHTML").Contains("New chat"))
                {
                    button.Click();
                    break;
                }
            }

            driver.FindElementByClassName("jN-F5").SendKeys("123");

            foreach (var item in lstCheck)
            {
                string contato = item.Substring(item.IndexOf("_1wjpf\">") + 8, item.IndexOf("<", item.IndexOf("_1wjpf\">")) - item.IndexOf("_1wjpf\">") - 8);
                string status = item.Substring(item.IndexOf("data-icon=\"") + 11, item.IndexOf("\"", item.IndexOf("data-icon=\"") + 11) - item.IndexOf("data-icon=\"") - 11);
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Contato: " + contato + " Status: " + status);
            }
        }

        static List<string> lstCheck = new List<string>();

        private void checkStatus()
        {
            lstCheck.Clear();

            string style = driver.FindElementByClassName("RLfQR").GetAttribute("style");
            int height = int.Parse(style.Substring(style.IndexOf("height: ") + 8, style.IndexOf("px", style.IndexOf("height: ")) - (style.IndexOf("height: ") + 8)));

            for (int i = 0; i < height; i += 250)
            {
                //document.getElementsByClassName("_1NrpZ")[0].scrollTo(0, 1000)
                driver.ExecuteScript("document.getElementsByClassName(\"_1NrpZ\")[0].scrollTo(0, " + i + ")", null);
                //System.Threading.Thread.Sleep(500);
                var x = driver.FindElementsByClassName("_3j7s9");

                foreach (var item in x)
                {
                    if (item.GetAttribute("innerHTML").Contains("_1VfKB"))
                    {
                        if (!lstCheck.Contains(item.GetAttribute("innerHTML")))
                            lstCheck.Add(item.GetAttribute("innerHTML"));
                    }
                }

            }
        }
    }
}

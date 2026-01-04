using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.Console
{
    public class SeleniumHelper
    {
        ChromeDriver driver;

        public SeleniumHelper(ChromeDriver driver)
        {
            this.driver = driver;
        }


        public bool waitForInnerHtml(string value)
        {
            DateTime timeout = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if (DateTime.Now > timeout)
                    break;

                if (hasInnerHtml(value))
                    return true;

                System.Threading.Thread.Sleep(10);
            }

            return false;
        }

        public bool waitForInnerHtml(OpenQA.Selenium.IWebElement element, string value)
        {
            DateTime timeout = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if (DateTime.Now > timeout)
                    break;

                if (hasInnerHtml(element, value))
                    return true;

                System.Threading.Thread.Sleep(10);
            }

            return false;
        }

        public bool waitForInnerText(OpenQA.Selenium.IWebElement element, string value)
        {
            DateTime timeout = DateTime.Now.AddSeconds(10);
            while (true)
            {
                if (DateTime.Now > timeout)
                    break;

                if (hasInnerText(element, value))
                    return true;

                System.Threading.Thread.Sleep(10);
            }

            return false;
        }


        public bool hasInnerHtml(string value)
        {
            try
            {
                if (this.driver.PageSource != null && this.driver.PageSource.Contains(value))
                return true;
            }
            catch (Exception e)
            {
                // TODO - Grava erro no banco de dados para analise;
                //cod. 02-431
                return false;                
            }
            return false;
        }

        public bool hasInnerHtml(OpenQA.Selenium.IWebElement element, string value)
        {
            try
            {
                string innerHtml = element.GetAttribute("innerHTML");

                if (innerHtml != null && innerHtml.Contains(value))
                    return true;
            }
            catch(Exception e)
            {
                // TODO - Grava erro no banco de dados para analise;
                //cod. 02-315
                return false;
            }

            return false;
        }

        public bool hasInnerText(OpenQA.Selenium.IWebElement element, string value)
        {
            try
            {
                if (this.driver.PageSource != null && element.Text.Contains(value))
                    return true;
            }
            catch(Exception e)
            {
                //cod. 02-354
                return false;
            }
            return false;
        }
    }
}

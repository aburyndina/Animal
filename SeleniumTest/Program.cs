using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Extensions;

namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var driver3 = new ChromeDriver("");
            var driver = new ChromeDriver(@"C:\Program Files\2.23.1\net40\chromedriver_win32", new ChromeOptions(){});
            //var driver = new InternetExplorerDriver(@"C:\Program Files\2.23.1");
            string baseURL = "http://localhost:31872/Home/Index";
            driver.Navigate().GoToUrl(baseURL);
            
            driver.FindElement(By.Id("TypeId")).FindElement(By.CssSelector("option[value='1']")).Click();
           
            //поиск
            IWebElement searchButton = driver.FindElement(By.XPath("//input[@type='submit' and @value='Найти']"));
            searchButton.Click();

            //создание животного
            IWebElement addAnimal = driver.FindElement(By.XPath("//input[@type='submit' and @value='Создать животное']"));
            addAnimal.Click();
            Thread.Sleep(100);

            driver.FindElement(By.LinkText("Обратно")).Click();
            Thread.Sleep(100);
            driver.FindElement(By.XPath("//input[@type='submit' and @value='Создать животное']")).Click();
            Thread.Sleep(100);

            var animalName = driver.FindElement(By.Id("AnimalName"));
            animalName.Click();
            animalName.Clear();
            string anName = "Пантера";
            animalName.SendKeys(anName);
            string xpathAnimalType = string.Format(".//option[contains(text(), '{0}' )]", "Млекопитающие");
            driver.FindElement(By.Id("TypeId")).FindElement(By.XPath(xpathAnimalType)).Click();
            driver.FindElement(By.Id("FellColorId")).FindElement(By.XPath(".//option[contains(text(),'черная')]")).Click();
            driver.FindElement(By.Id("RegionId")).FindElement(By.XPath(".//option[contains(text(),'Германия')]")).Click();
            Thread.Sleep(100);
            driver.FindElement(By.Id("LocationId")).FindElement(By.XPath(".//option[contains(text(),'Кельн')]")).Click();
            driver.FindElement(By.XPath("//input[@type='submit' and @value='Сохранить']")).Click();
            Thread.Sleep(100);

            //Поиск с checkbox
            var result = driver.ExecuteJavaScript<string>("var rl = $('#regionList'); rl.multiselect('open'); rl.multiselect('uncheckAll'); return '0';");
            driver.FindElement(By.XPath(".//span[contains(text(),'Германия')]")).Click();

            driver.FindElement(By.Id("TypeId")).FindElement(By.XPath(".//option[contains(text(),'Млекопитающие')]")).Click();
            driver.FindElement(By.Id("FellColorId")).FindElement(By.XPath(".//option[contains(text(),'черная')]")).Click();
            driver.FindElement(By.XPath("//input[@type='submit' and @value='Найти']")).Click();
            Thread.Sleep(100);
            
            IWebElement animalTable = driver.FindElement(By.Id("Animal"));
            ReadOnlyCollection<IWebElement> allRows = animalTable.FindElements(By.TagName("tr"));
            var deleteLink = new List<string>();
            for (int i = 0; i < allRows.Count; i++)
            {
                ReadOnlyCollection<IWebElement> cells = allRows[i].FindElements(By.TagName("td"));
                
                for (int j = 0; j < cells.Count; j++)
                {
                    if (j > 1) break;
                    var value = allRows[i].FindElements(By.TagName("td"))[j];

                    if (j == 0 && value.Text==anName)
                    {
                       // deleteLinkId = value.GetAttribute("href");
                        //deleteLinkId = allRows[i].FindElements(By.LinkText("Удалить"));
                        deleteLink.Add(allRows[i].FindElement(By.LinkText("Удалить")).GetAttribute("href"));
                        
                    }
                    if (j == 1 && !value.Text.Equals("Млекопитающие"))
                    {
                        Console.WriteLine("Неверный тип животного");
                    }
                }
            }

            var links = animalTable
                .FindElements(By.TagName("tr"))
                .Where(tr => tr.FindElements(By.TagName("td")).Any(td => td.Text == anName))
                .Select(tr => tr.FindElement(By.LinkText("Удалить")).GetAttribute("href"))
                .ToList();

  
            //удаляем животное
            //var deleteLink6 = string.Format("[href*='/Home/DeleteAnimal/{0}']", 1);
            deleteLink.ForEach(d => driver.Navigate().GoToUrl(d));
            //driver.Navigate().GoToUrl(deleteLinkId);
            //driver.FindElement(By.CssSelector(deleteLinkId)).Click();

            driver.Close();
            Console.WriteLine("Finish");
            Console.ReadKey();

            //By bycss = By.CssSelector("input[type='checkbox'][name='vm-video-select-all']");
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //IWebElement myDynamicElement = wait.Until<IWebElement>((d) =>
            //{
            //    return d.FindElements(bycss).ToList().Find(e => e.Displayed);
            //});

            //myDynamicElement.Click();
        }
    }
}

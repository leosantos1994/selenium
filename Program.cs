using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static Configuracao Confg;
        static Apuracao Apuracao;
        static DateTime Inicio;
        static DateTime Final;

        public static void Main(string[] args)
        {
            Confg = new Configuracao();
            Apuracao = new Apuracao();

            List<string> todosOsVotantes = Confg.GetUsuarios();

            Inicio = DateTime.Now;

            List<Thread> threadsEmExucucao = new List<Thread>();
            for (int i = 0; i < Confg.Navegadores; i++)
            {
                List<string> votantesDoNavegador = todosOsVotantes.Splice(0, Confg.VotosPorNavegador);
                Thread thr = new Thread(() => RegistraVoto(votantesDoNavegador));
                threadsEmExucucao.Add(thr);
                thr.Start();
            }

            while (threadsEmExucucao.Any(x => x.IsAlive))
                Thread.Sleep(1000);
            Final = DateTime.Now;
            Apurar();
        }

        public static void Apurar()
        {
            Console.WriteLine("cls");
            Console.WriteLine($"INICIO: {Inicio} - FIM: {Final}");
            Console.WriteLine("RESULTADO DA VOTAÇÃO");
            Console.WriteLine($"BRANCOS: {Apuracao.VotosBrancos}");
            Console.WriteLine($"NULOS: {Apuracao.VotosNulos}");
            foreach (var chapa in Apuracao.VotosChapas)
            {
                Console.WriteLine($"CHAPA{chapa.Key}: {chapa.Value}");
            }
        }

        public static void Login(IWebDriver driver, WebDriverWait await, string userID, string userPass)
        {
            await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_BTConfirma"))).Click();
            driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
            driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
            await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma"))).Click();
        }


        public static void TrocarSenha(IWebDriver driver, WebDriverWait await, string userID, string userPass)
        {
            try
            {
                driver.FindElement(By.Id("_ctl0_MainContent_IDSenhaNova")).SendKeys(userPass);
                driver.FindElement(By.Id("_ctl0_MainContent_IDConfirmaSenha")).SendKeys(userPass);
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_proximo"))).Click();

                driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
                driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma"))).Click();
            }
            catch (Exception ex) { }
        }

        public static void Votar(IWebDriver driver, WebDriverWait await, int voto)
        {
            try
            {
                await.Until(x => driver.FindElement(By.Id("votar"))).Click();
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();
                if (voto == 4)
                {
                    await.Until(x => driver.FindElement(By.Id("bt_branco"))).Click();
                }
                else if (voto == 5)
                {
                    await.Until(x => driver.FindElement(By.Id("bt_nulo"))).Click();
                }
                else
                {
                    driver.FindElement(By.Id("_ctl0_MainContent_NumVotacao")).SendKeys(voto.ToString());
                    await.Until(x => driver.FindElement(By.Id("bt_confirma"))).Click();
                }

                Apuracao.SetVoto(voto);

                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_btnConfirmar"))).Click();
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();
                await.Until(x => driver.FindElement(By.Id("sair"))).Click();
            }
            catch (Exception ex) { }
        }

        public static void RegistraVoto(List<string> users)
        {
            IWebDriver driver;
            WebDriverWait await;

            driver = new ChromeDriver();
            await = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                string userId = "";
                for (int i = 0; i < users.Count; i++)
                {
                    userId = users[i];
                    driver.Navigate().GoToUrl(Confg.URL);
                    int voto = new Random().Next(1, 5);

                    Login(driver, await, userId, Confg.SenhaPadrao);
                    TrocarSenha(driver, await, userId, Confg.SenhaPadrao);
                    Votar(driver, await, voto);
                }
            }
            catch { }
            finally
            {
                driver.Close();
            }
        }
    }
}


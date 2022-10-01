using RoboVotacao.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace RoboVotacao
{
    class Program
    {
        static Configuracao Confg;
        static Apuracao Apuracao;
        static DateTime Inicio;
        static DateTime Final;

        public static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    try
                    {
                        IniciarConfiguracoes();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        goto final;
                    }
                    ExecutarRobo();
                    Apurar(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Apurar(true);
                    break;
                }
            }

            //mantem o prompt abero
            final: Console.ReadLine();
        }

        public static void IniciarConfiguracoes()
        {

            while (!Init.Iniciou()) { Console.WriteLine("Aguardando inicio!"); };

            var configInicial = Init.BuscarConfiguracaoInicial();

            if (configInicial is null || !string.IsNullOrEmpty(configInicial.MensagemRetorno))
                throw new Exception(configInicial?.MensagemRetorno ?? "Erro ao baixar as configurações iniciais do robô.");

            Confg = new Configuracao(configInicial);
            Console.WriteLine("Configurações baixadas iniciando votação!");

        }

        public static void ExecutarRobo()
        {
            try
            {

                Apuracao = new Apuracao();

                Inicio = DateTime.Now;

                var arr = new Dictionary<long, List<VotanteExportar>>();

                int qtd = Confg.Usuarios.Count / Confg.Navegadores;
                for (int i = 0; i < Confg.Navegadores; i++)
                {
                    var lista = Confg.Usuarios.Splice(0, (qtd == 0 ? Confg.Usuarios.Count : qtd));
                    if (lista.Count > 0)
                        arr.Add(i, lista);
                }

                Parallel.ForEach(arr, (data) =>
                {
                    new ExecucaoRobo(Confg, ref Apuracao).RegistraVotoCompleto(data.Value);
                });

            }
            catch (Exception ex) { Console.WriteLine("Ocorreu uma falha no processo: " + ex.Message); }
            finally
            {
                Final = DateTime.Now;
                Console.WriteLine("Irá buscar votantes pendentes.");
            }


        }

        public static void Apurar(bool end)
        {
            Console.WriteLine("cls");
            Console.WriteLine($"INICIO: {Inicio} - FIM: {Final}");
            Console.WriteLine(!end ? "VOTOS COMPUTADOS ATÉ AGORA" : "RESULTADO DA VOTAÇÃO");
            Console.WriteLine($"BRANCOS: {Apuracao?.VotosBrancos ?? 0}");
            Console.WriteLine($"NULOS: {Apuracao?.VotosNulos ?? 0}");
            foreach (var chapa in GetListaVotosChapa(Apuracao?.VotosChapas))
            {
                Console.WriteLine($"CHAPA{chapa.Key}: {chapa.Value}");
            }
        }

        private static Dictionary<int, int> GetListaVotosChapa(Dictionary<int, int> votos)
        {
            if (votos is null)
            {
                var dic = new Dictionary<int, int>();
                dic.Add(0, 0);
                return dic;
            }
            return votos;
        }

        public static void Login(IWebDriver driver, WebDriverWait await, string userID, string userPass, int regional)
        {
            //Seleciona a eleição/regional
            var select = await.Until(x => driver.FindElement(By.Id("CBEleicoes")));
            var selectElement = new SelectElement(select);
            selectElement.SelectByValue(regional.ToString());
            await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_BTConfirma"))).Click();

            //Faz o primeiro Login
            driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
            driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
            await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma"))).Click();
        }

        public static void PerguntasVotacao(IWebDriver driver, WebDriverWait await, VotanteExportar votante)
        {

        }

        public static void TrocarSenha(IWebDriver driver, WebDriverWait await, string userID, string userPass)
        {
            try
            {
                //Seta os campos da nova senha 
                driver.FindElement(By.Id("_ctl0_MainContent_IDSenhaNova")).SendKeys(userPass);
                driver.FindElement(By.Id("_ctl0_MainContent_IDConfirmaSenha")).SendKeys(userPass);
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_proximo"))).Click();

                //Faz o segundo login
                driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
                driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma"))).Click();
            }
            catch (Exception ex) { }
        }

        public static void Votar(IWebDriver driver, WebDriverWait await, TipoVoto voto, int numDeChapas)
        {
            try
            {
                //para cada votação configurada
                //Os votos serão iguais para cada votação
                for (int i = 0; i < Confg.NumeroDeVotacoes; i++)
                {
                    int votoChapa = 0;
                    await.Until(x => driver.FindElement(By.Id("votar"))).Click();
                    await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();
                    if (voto == TipoVoto.Branco)
                    {
                        await.Until(x => driver.FindElement(By.Id("bt_branco"))).Click();
                    }
                    else if (voto == TipoVoto.Nulo)
                    {
                        await.Until(x => driver.FindElement(By.Id("bt_nulo"))).Click();
                    }
                    else
                    {
                        votoChapa = new Random().Next(1, numDeChapas);
                        driver.FindElement(By.Id("_ctl0_MainContent_NumVotacao")).SendKeys(votoChapa.ToString());
                        await.Until(x => driver.FindElement(By.Id("bt_confirma"))).Click();
                    }
                    Apuracao.SetVoto(voto, votoChapa);
                    await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_btnConfirmar"))).Click();
                }

                await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();
                await.Until(x => driver.FindElement(By.XPath("//*[normalize-space() = 'Sair']"))).Click();
            }
            catch (Exception ex) { }
        }

        public static void RegistraVoto(List<VotanteExportar> users)
        {
            IWebDriver driver;
            WebDriverWait await;

            driver = new ChromeDriver();
            await = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                for (int i = 0; i < users.Count; i++)
                {
                    driver.Navigate().GoToUrl(Confg.URL);

                    Login(driver, await, users[i].CPF, Confg.SenhaPadrao, users[i].Regional);
                    PerguntasVotacao(driver, await, users[i]);
                    TrocarSenha(driver, await, users[i].CPF, Confg.SenhaTroca);
                    Votar(driver, await, users[i].TipoVoto, Confg.NumeroDeChapas);
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


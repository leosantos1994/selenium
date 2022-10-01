using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RoboVotacao;
using RoboVotacao.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboVotacao
{
    public class ExecucaoRobo
    {
        Configuracao Confg;
        Apuracao Apuracao;
        public ExecucaoRobo(Configuracao conf, ref Apuracao apuracao)
        {
            Confg = conf;
            Apuracao = apuracao;
        }

        public void RegistraVotoCompleto(List<VotanteExportar> users)
        {
            ChromeDriver driver = null;
            driver = new ChromeDriver(ConfigurationManager.AppSettings.Get("DriverPath"), new ChromeOptions() { PageLoadStrategy = PageLoadStrategy.Eager }, new TimeSpan(0, 0, 15));
            WebDriverWait await = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
            driver.Manage().Window.Maximize();

            foreach (var votante in users)
            {
                try
                {
                    Init.AddVotanteOnline();
                    driver.Navigate().GoToUrl(Confg.URL);

                    /*Login*/
                    string senha = votante.Status == StatusVotante.TrocouSenha ? Confg.SenhaTroca : Confg.SenhaPadrao;
                    Login(ref driver, await, votante.CPF, senha, votante.Regional);

                    if (driver.Url.Contains("Valida.aspx"))
                        ResponderPergunta(ref driver, await, votante);

                    /*Trocar senha*/
                    if (driver.Url.Contains("AlteraSenha"))
                    {
                        TrocarSenha(ref driver, await, votante.CPF, Confg.SenhaTroca);
                        Init.AtualizarStatusVotante(votante.ID, StatusVotante.TrocouSenha);
                    }

                    if (driver.Url.Contains("Validacao.aspx"))
                    {
                        driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime")).Click();
                    }
                    else if (driver.Url.Contains("Home"))
                    {
                        await.Until(x => driver.FindElement(By.Id("votar"))).Click();
                        await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();
                    }

                    /*Votar*/
                    Votar(driver, await, votante.TipoVoto, Confg.NumeroDeChapas);

                    Init.AtualizarStatusVotante(votante.ID, StatusVotante.Votou);

                    Init.RemoveVotanteOnline();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRRRRROOOO   =====================================================================================" +
                            "=========================================================  " + ex.Message);
                    Screenshot ss = driver.GetScreenshot();

                    ss.SaveAsFile(Path.Combine(Confg.SSPath, (votante.Nome + "-" + DateTime.Now.ToString("dd-MM-hh-mm-s") + ".png").Replace(" ", "")), ScreenshotImageFormat.Png);
                    Init.RemoveVotanteOnline();
                    continue;
                }
            }
            driver.Close();
        }

        public void Login(ref ChromeDriver driver, WebDriverWait await, string userID, string userPass, int regional)
        {
            await = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            //Seleciona a eleição/regional
            var select = driver.FindElement(By.Id("CBEleicoes"));
            var selectElement = new SelectElement(select);
            selectElement.SelectByValue(regional.ToString());
            driver.FindElement(By.Id("_ctl0_MainContent_BTConfirma")).Click();

            //Faz o primeiro Login
            driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
            driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
            driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma")).Click();
        }

        public void ResponderPergunta(ref ChromeDriver driver, WebDriverWait await, VotanteExportar votante)
        {
            try
            {
                do
                {
                    string textopergunta = driver.FindElement(By.TagName("h4")).Text;
                    var perguntaResposta = Confg.PerguntaResposta.FirstOrDefault(x => x.Key == textopergunta);

                    var rdOpcao1 = driver.FindElement(By.XPath("//label[@for='_ctl0_MainContent_rdOpcao1']")).Text;
                    var rdOpcao2 = driver.FindElement(By.XPath("//label[@for='_ctl0_MainContent_rdOpcao2']")).Text;
                    var rdOpcao3 = driver.FindElement(By.XPath("//label[@for='_ctl0_MainContent_rdOpcao3']")).Text;
                    var rdOpcao4 = driver.FindElement(By.XPath("//label[@for='_ctl0_MainContent_rdOpcao4']")).Text;

                    switch (perguntaResposta.Value)
                    {
                        case CapoRespostaPergunta.Null:
                            throw new Exception("Erro ao responder pergunta, nenhuma resposta foi localizada");

                        case CapoRespostaPergunta.DataNascimento:
                            {
                                if (rdOpcao1 == votante.DataNascimento)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao2 == votante.DataNascimento)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao3 == votante.DataNascimento)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao4 == votante.DataNascimento)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                            }
                            break;
                        case CapoRespostaPergunta.CPF:
                            {
                                if (rdOpcao1 == votante.CPF)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao2 == votante.CPF)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao3 == votante.CPF)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao4 == votante.CPF)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                            }
                            break;
                        case CapoRespostaPergunta.NomeMae:
                            {
                                if (rdOpcao1 == votante.NomeMae)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao2 == votante.NomeMae)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao3 == votante.NomeMae)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                                else if (rdOpcao4 == votante.NomeMae)
                                    driver.ExecuteScript("$('#_ctl0_MainContent_rdOpcao1').click()");
                            }
                            break;
                        default:
                            break;
                    }
                    driver.FindElement(By.Id("_ctl0_MainContent_button2")).Click();
                }
                while (driver.Url.Contains("Valida"));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao responder pergunta: Votante: " + votante.Nome + " CPF: " + votante.CPF);
            }
        }
        public void TrocarSenha(ref ChromeDriver driver, WebDriverWait await, string userID, string userPass)
        {
            //Seta os campos da nova senha 
            driver.FindElement(By.Id("_ctl0_MainContent_IDSenhaNova")).SendKeys(userPass);
            driver.FindElement(By.Id("_ctl0_MainContent_IDConfirmaSenha")).SendKeys(userPass);
            driver.FindElement(By.Id("_ctl0_MainContent_bt_proximo")).Click();

            //Faz o segundo login
            driver.FindElement(By.Id("_ctl0_MainContent_IDRegistro")).SendKeys(userID);
            driver.FindElement(By.Id("_ctl0_MainContent_Password")).SendKeys(userPass);
            driver.FindElement(By.Id("_ctl0_MainContent_bt_confirma")).Click();
        }

        public void Votar(ChromeDriver driver, WebDriverWait await, TipoVoto voto, int numDeChapas)
        {
            //para cada votação configurada
            //Os votos serão iguais para cada votação
            for (int i = 0; i < Confg.NumeroDeVotacoes; i++)
            {
                int votoChapa = 0;

                if (voto == TipoVoto.Branco)
                {
                    await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_NumVotacao"))).SendKeys("00");
                    //await.Until(x => driver.FindElement(By.Id("bt_confirma"))).Click();
                    driver.ExecuteScript("$('#bt_confirma').click()");
                    //await.Until(x => driver.FindElement(By.Id("bt_branco"))).Click();
                }
                else if (voto == TipoVoto.Nulo)
                {
                    await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_NumVotacao"))).SendKeys("09");
                    //await.Until(x => driver.FindElement(By.Id("bt_confirma"))).Click();
                    driver.ExecuteScript("$('#bt_confirma').click()");

                    //await.Until(x => driver.FindElement(By.Id("bt_nulo"))).Click();
                }
                else
                {
                    votoChapa = Confg.ChapaVotar > 0 ? Confg.ChapaVotar : new Random().Next(1, numDeChapas);
                    await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_NumVotacao"))).SendKeys(votoChapa.ToString());
                    //await.Until(x => driver.FindElement(By.Id("bt_confirma"))).Click();
                    driver.ExecuteScript("$('#bt_confirma').click()");

                }
                Apuracao.SetVoto(voto, votoChapa);
                //await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_btnConfirmar"))).Click();
                driver.ExecuteScript("$('#_ctl0_MainContent_btnConfirmar').click()");

            }
            await.Until(x => driver.FindElement(By.Id("_ctl0_MainContent_bt_imprime"))).Click();

            await.Until(x => driver.FindElement(By.XPath("//*[normalize-space() = 'Sair']"))).Click();
            //await.Until(x => driver.FindElement(By.Id("Sair"))).Click();
        }
    }
}

using RoboVotacao.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoboVotacao
{
    public class Configuracao
    {
        public int Navegadores { get; } = 1;
        public int VotosPorNavegador { get; } = 25;
        public string SenhaPadrao { get; } = "";
        public string SenhaTroca { get; } = "";
        public int NumeroDeChapas { get; } = 1;
        public int NumeroDeVotacoes { get; } = 1;
        public int ChapaVotar { get; } = 0;
        public string URL { get; } = "";
        public Dictionary<string, CapoRespostaPergunta> PerguntaResposta { get; set; }
        public List<VotanteExportar> Usuarios { get; }
        public string SSPath { get; } = Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "ScreenShots");

        public Configuracao(ConfiguracaoInicialRobo model)
        {
            Usuarios = model.Votantes;
            SenhaPadrao = model.SenhaPadrao;
            SenhaTroca = model.SenhaTroca;
            Navegadores = model.Robo.Navegadores;
            URL = model.URL;
            NumeroDeChapas = model.NumeroDeChapas;
            NumeroDeVotacoes = model.NumeroDeVotacoes;
            ChapaVotar = model.Robo.Chapa;
            PerguntaResposta = model.PerguntaResposta ?? null;

            if (!Directory.Exists(SSPath))
                Directory.CreateDirectory(SSPath);
        }
    }
}

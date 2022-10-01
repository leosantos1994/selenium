using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboVotacao.Model
{
    public class ConfiguracaoInicialRobo
    {
        public string MensagemRetorno { get; set; }
        public string Status { get; set; }
        public int NumeroDeChapas { get; } = 3;
        public int NumeroDeVotacoes { get; set; }
        public string SenhaPadrao { get; set; }
        public string SenhaTroca { get; set; }
        public string URL { get; set; }
        public Dictionary<string, CapoRespostaPergunta> PerguntaResposta { get; set; } 
        public List<VotanteExportar> Votantes { get; set; }
        public Robo Robo { get; set; }
    }

    public class VotanteExportar
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string NomeMae { get; set; }
        public string DataNascimento { get; set; }
        public int Regional { get; set; }
        public TipoVoto TipoVoto { get; set; }
        public StatusVotante Status { get; set; }
    }

    public class Robo
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int QtdVotosBranco { get; set; }
        public int QtdVotosNulo { get; set; }
        public int QtdVotos { get; set; }
        public int Chapa { get; set; }
        public int Navegadores { get; set; }
        public int Regional { get; set; }
        public String UF { get; set; }
        public System.DateTime? CriadoEm { get; set; }
        public System.DateTime? AtualizadoEm { get; set; }
    }

    public enum TipoVoto
    {
        Branco,
        Nulo,
        Chapa
    }

    public enum StatusVotante
    {
        Null,
        TrocouSenha,
        Votou
    }

    public enum CapoRespostaPergunta
    {
        Null,
        DataNascimento,
        CPF,
        NomeMae
    }
}

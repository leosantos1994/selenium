using RoboVotacao.Model;
using System.Collections.Generic;

namespace RoboVotacao
{
    public class Apuracao
    {
        ApuracaoModel interna { get; set; } = new ApuracaoModel();


        public int VotosBrancos { get { return interna.Branco; } }
        public int VotosNulos { get { return interna.Nulo; } }
        public Dictionary<int, int> VotosChapas { get { return interna.ChapasVotos; } }

        public void SetVoto(TipoVoto voto, int votoChapa)
        {
            lock (interna)
            {
                if (voto == TipoVoto.Branco)
                    SetBranco();
                else if (voto == TipoVoto.Nulo)
                    SetNulo();
                else SetVotoChapa(votoChapa);
            }
        }

        void SetBranco()
        {
            interna.Branco++;
        }

        void SetNulo()
        {
            interna.Nulo++;
        }

        void SetVotoChapa(int voto)
        {
            if (interna.ChapasVotos.ContainsKey(voto))
                interna.ChapasVotos[voto] += 1;
            else interna.ChapasVotos.Add(voto, 1);
        }

        private class ApuracaoModel
        {
            public int Branco { get; set; } = 0;
            public int Nulo { get; set; } = 0;
            public Dictionary<int, int> ChapasVotos { get; set; } = new Dictionary<int, int>();
        }
    }
}

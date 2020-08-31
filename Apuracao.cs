using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Apuracao
    {
        ApuracaoModel interna { get; set; } = new ApuracaoModel();


        public int VotosBrancos { get { return interna.Branco; } }
        public int VotosNulos { get { return interna.Nulo; } }
        public Dictionary<int, int> VotosChapas { get { return interna.ChapasVotos; } }

        public void SetVoto(int voto)
        {
            lock (interna)
            {
                if (voto == 4)
                    SetBranco();
                else if (voto == 5)
                    SetNulo();
                else SetVotoChapa(voto);
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

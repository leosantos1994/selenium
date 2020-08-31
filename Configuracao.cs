using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Configuracao
    {
        public int Navegadores { get; } = 4;
        public int VotosPorNavegador { get; } = 25;
        public string SenhaPadrao { get; } = "123Mudar";
        public string URL { get; } = "https://cfp_hml.brctotal.com/BREleicoes_H/eleicoes/EscolherEleicao.aspx";
        private List<string> Usuarios { get; set; }

        public Configuracao()
        {
            SetUsers();
        }

        public List<string> GetUsuarios()
        {
            return Usuarios;
        }

        private void SetUsers()
        {
            Usuarios = new List<string>();
            Usuarios.Add("888.185.438-88");
            Usuarios.Add("777.074.336-77");
            Usuarios.Add("555.754.224-55");
            Usuarios.Add("444.644.123-44");
            Usuarios.Add("333.434.021-33");
            Usuarios.Add("111.314.828-11");
            Usuarios.Add("000.103.716-00");
            Usuarios.Add("888.083.614-88");
            Usuarios.Add("666.763.512-66");
            Usuarios.Add("555.653.410-55");
            Usuarios.Add("777.274.727-77");
            Usuarios.Add("666.064.626-66");
            Usuarios.Add("444.844.524-44");
            Usuarios.Add("333.634.422-33");
            Usuarios.Add("222.523.310-22");
            Usuarios.Add("000.303.217-00");
            Usuarios.Add("888.283.115-88");
            Usuarios.Add("777.073.014-77");
            Usuarios.Add("555.852.802-55");
            Usuarios.Add("444.742.600-44");
            Usuarios.Add("222.522.507-22");
            Usuarios.Add("111.412.405-11");
            Usuarios.Add("000.202.383-00");
            Usuarios.Add("777.171.282-77");
            Usuarios.Add("666.861.180-66");
            Usuarios.Add("555.751.087-55");
            Usuarios.Add("333.531.875-33");
            Usuarios.Add("222.421.773-22");
            Usuarios.Add("111.210.671-11");
            Usuarios.Add("888.180.570-88");
            Usuarios.Add("777.870.467-77");
            Usuarios.Add("666.760.365-66");
            Usuarios.Add("444.548.263-44");
            Usuarios.Add("222.824.332-22");
            Usuarios.Add("000.603.230-00");
            Usuarios.Add("888.583.137-88");
            Usuarios.Add("777.373.036-77");
            Usuarios.Add("555.253.824-55");
            Usuarios.Add("444.042.722-44");
            Usuarios.Add("222.822.620-22");
            Usuarios.Add("111.612.527-11");
            Usuarios.Add("000.502.415-00");
            Usuarios.Add("777.372.314-77");
            Usuarios.Add("666.261.212-66");
            Usuarios.Add("555.051.110-55");
            Usuarios.Add("333.831.007-33");
            Usuarios.Add("222.721.805-22");
            Usuarios.Add("111.511.703-11");
            Usuarios.Add("888.480.602-88");
            Usuarios.Add("777.270.480-77");
            Usuarios.Add("666.160.387-66");
            Usuarios.Add("444.840.285-44");
            Usuarios.Add("333.738.183-33");
            Usuarios.Add("222.528.071-22");
            Usuarios.Add("000.408.870-00");
            Usuarios.Add("888.288.777-88");
            Usuarios.Add("777.178.675-77");
            Usuarios.Add("555.857.563-55");
            Usuarios.Add("444.747.461-44");
            Usuarios.Add("333.637.368-33");
            Usuarios.Add("111.417.267-11");
            Usuarios.Add("000.307.155-00");
            Usuarios.Add("888.186.053-88");
            Usuarios.Add("000.608.371-00");
            Usuarios.Add("888.488.278-88");
            Usuarios.Add("777.377.166-77");
            Usuarios.Add("555.257.064-55");
            Usuarios.Add("444.047.862-44");
            Usuarios.Add("333.837.751-33");
            Usuarios.Add("111.616.658-11");
            Usuarios.Add("000.506.556-00");
            Usuarios.Add("888.386.454-88");
            Usuarios.Add("666.266.342-66");
            Usuarios.Add("555.056.240-55");
            Usuarios.Add("444.845.148-44");
            Usuarios.Add("222.625.046-22");
            Usuarios.Add("111.515.734-11");
            Usuarios.Add("000.305.632-00");
            Usuarios.Add("777.275.530-77");
            Usuarios.Add("666.064.437-66");
            Usuarios.Add("222.620.375-22");
            Usuarios.Add("333.631.883-33");
            Usuarios.Add("111.713.362-11");
            Usuarios.Add("000.503.260-00");
            Usuarios.Add("777.473.157-77");
            Usuarios.Add("666.262.055-66");
            Usuarios.Add("555.152.853-55");
            Usuarios.Add("333.832.752-33");
            Usuarios.Add("222.722.640-22");
            Usuarios.Add("111.511.547-11");
            Usuarios.Add("888.481.445-88");
            Usuarios.Add("777.271.343-77");
            Usuarios.Add("666.161.231-66");
            Usuarios.Add("444.041.130-44");
            Usuarios.Add("333.730.037-33");
            Usuarios.Add("222.620.835-22");
            Usuarios.Add("000.400.723-00");
            Usuarios.Add("888.380.621-88");
            Usuarios.Add("777.178.528-77");
            Usuarios.Add("555.058.427-55");
        }

    }
}

using ClientBLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog
{
     public static class Util
     {
        public static User user = new User();
        public static IServiceProvider services;
        public static bool usuarioLogado = false;

        public enum PostagemOpcoes
        {
            CriarNovaPostagem = 1,
            AlterarPostagem = 2,
            DeletarPostagem = 3,
            ListarPostagens = 4
        }
    }
}

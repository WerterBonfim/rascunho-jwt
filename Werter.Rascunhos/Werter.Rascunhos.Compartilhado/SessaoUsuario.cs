using System;

namespace Werter.Rascunhos.Compartilhado
{
    public sealed class SessaoUsuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
using System;

namespace ApiPetShop.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Nome { get; set; }               
        public string Login { get; set; }              
        public string Senha { get; set; }              
        public string Token { get; set; }             
        public bool Ativo { get; set; }             

        public Usuario()
        {
            Nome = string.Empty;
            Login = string.Empty;
            Senha = string.Empty;
            Token = string.Empty;
            Ativo = false;
        }
    }
}

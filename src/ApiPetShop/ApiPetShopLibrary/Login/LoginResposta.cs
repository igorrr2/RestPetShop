namespace ApiPetShopLibrary.Login
{
    public class LoginResposta
    {
        public int statusCode { get; set; }
        public string MensagemRetorno { get; set; }
        public string UsuarioNome { get; set; }
        public string TokenAutenticacao { get; set; }
        public string UsuarioId { get; set; }
    }
}

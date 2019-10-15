//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe Model de resposta do WebApi de Login
//  Autor             : Edson Oliveira
//  Data da Criação   : 06/02/2019
//  Projeto           : 11010
//
//  ==============================================================================
//
//  Alterado Por      : 
//  Data da Alteração : 
//  Projeto           : 
//  Descrição         : 
//
//*********************************************************************************


namespace CheckCredentials.Model
{
    public class LoginResponseModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public bool CredentialAccepted { get; set; }
        public string ErrorMessage { get; set; }
    }
}

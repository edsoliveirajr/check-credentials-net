//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe Model login para requisição do WebApi
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
    public class LoginRequestModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
    }
}

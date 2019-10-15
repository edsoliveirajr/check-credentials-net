//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Global asax do WebApi
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

using System.Web.Http;

namespace CheckCredentials.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe WebAPI para validação do login 
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

using CheckCredentials.BLL;
using CheckCredentials.Model;
using System.Web.Http;

namespace CheckCredentials.WebApi.Controllers
{
    public class ValidateLoginController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult PostValidadeLogin(LoginRequestModel loginRequest)
        {
            LoginBiz loginBiz = new LoginBiz();
            LoginResponseModel loginResponse = loginBiz.ValidateLogin(loginRequest);

            return Json(loginResponse);
        }
    }
}

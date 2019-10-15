//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe Business para validação do login 
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


using CheckCredentials.DAL;
using CheckCredentials.Model;
using System;

namespace CheckCredentials.BLL
{
    public class LoginBiz
    {
        /// <summary>
        /// Valida o login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public LoginResponseModel ValidateLogin(LoginRequestModel loginRequest)
        {
            string msgErro = null;
            MainRepository repository = new MainRepository();

            try
            {
                // Valida o parâmetro de entrada
                CheckLoginRequest(loginRequest);

                // Valida a conexão com o banco de dados
                repository.CheckDatabaseConnection(loginRequest);

                // Valida o Login
                CheckLogin(loginRequest, repository);
            }
            catch (Exception ex)
            {
                msgErro = ex.Message;
            }

            // Define o objeto login de resposta
            LoginResponseModel loginResponse = new LoginResponseModel();
            loginResponse.Login = loginRequest?.Login;
            loginResponse.Password = loginRequest?.Password;
            loginResponse.Company = loginRequest?.Company;
            loginResponse.CredentialAccepted = String.IsNullOrEmpty(msgErro);
            loginResponse.ErrorMessage = msgErro;

            return loginResponse;
        }

        /// <summary>
        /// Valida o parâmetro LoginRequest
        /// </summary>
        /// <param name="loginRequest"></param>
        private static void CheckLoginRequest(LoginRequestModel loginRequest)
        {
            if (loginRequest == null)
            {
                throw new Exception("Parâmetro de entrada não foi informado!");
            }

            if (String.IsNullOrEmpty(loginRequest.Company))
            {
                throw new Exception("Company não foi informado!");
            }

            if (String.IsNullOrEmpty(loginRequest.Login))
            {
                throw new Exception("Login não foi informado!");
            }

            if (String.IsNullOrEmpty(loginRequest.Password))
            {
                throw new Exception("Password não foi informado!");
            }
        }

        /// <summary>
        /// Valida os dados do login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <param name="repository"></param>
        private static void CheckLogin(LoginRequestModel loginRequest, MainRepository repository)
        {
            LoginModel loginBD = repository.GetLoginData(loginRequest.Login);

            if (loginBD?.USUA_COD == null)
            {
                throw new Exception("Login não foi encontrado!");
            }

            if (!repository.IsPasswordValid(loginRequest.Password, loginBD.USUA_TXT_SEN))
            {
                throw new Exception("Senha informada incorretamente!");
            }

            if (loginBD.USUA_DAT_FIM != null)
            {
                throw new Exception("Usuário está desativado!");
            }

            if (!loginBD.SN_ACESIG)
            {
                throw new Exception("Usuário não tem acesso ao Signus-ERP!");
            }
        }
    }
}

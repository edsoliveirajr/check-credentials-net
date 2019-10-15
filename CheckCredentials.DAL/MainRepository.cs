//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe Repository para validação do login 
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


using Dapper;
using CheckCredentials.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace CheckCredentials.DAL
{
    public class MainRepository
    {
        // Obtém os conectionsString do arquivo de configuração
        protected IList<ConnectionStringSettings> connectionsString;
        protected SqlConnection connection;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainRepository()
        {
            this.connectionsString = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().ToList();
        }

        /// <summary>
        /// Valida se a conexão com o servidor de banco de dados
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public void CheckDatabaseConnection(LoginRequestModel loginRequest)
        {
            string msgErro = null;
            string database = null;

            // Verifica se existem connections string no web.config
            if (this.connectionsString == null || this.connectionsString.Count == 0)
                throw new Exception("Não foi encontrado uma string de conexão no Web.config");

            // Loop nos connections string
            foreach (var connectionString in this.connectionsString)
            {
                // Limpa a variável
                msgErro = null;

                // Criar um SqlConnectionStringBuilder do connection string
                SqlConnectionStringBuilder sqlConnectionStringBuilder =
                    new SqlConnectionStringBuilder(connectionString.ConnectionString);

                // Define o banco de dados no master
                sqlConnectionStringBuilder.InitialCatalog = "master";

                // Instancia a conexão de banco de dados
                try
                {
                    SqlConnection masterConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

                    // Define o nome do banco de dados
                    database = "SIGNUS_" + loginRequest.Company;

                    // Verifica se o banco de dados existe
                    if (!CheckDatabaseExists(masterConnection, database))
                    {
                        msgErro = $"Banco de dados {database} não existe no servidor de banco de dados";
                    }
                    else
                    {
                        // Define o banco de dados da company
                        sqlConnectionStringBuilder.InitialCatalog = database;

                        this.connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
                        break;
                    }
                   
                }
                catch (Exception ex)
                {
                    // Mensagem de erro ao criar uma connection string
                    msgErro = "Erro ao conectar no servidor de banco de dados: " + ex.Message;

                    // Se deu erro vai para o próximo registro
                    continue;
                }
            }

            // Se foi encontrado um erro, dispara um exception
            if (!String.IsNullOrEmpty(msgErro))
            {
                throw new Exception(msgErro);
            }
        }

        /// <summary>
        /// Verifica se o banco de dados existe
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private bool CheckDatabaseExists(SqlConnection masterConnection, string database)
        {
            string sql = string.Format(@"SELECT CONVERT(BIT, (CASE
                                                                WHEN EXISTS(SELECT 1
                                                                            FROM sys.databases
                                                                            WHERE name = '{0}') THEN 1
                                                                ELSE 0
                                                              END))", database);

            return masterConnection.QueryTrim<bool>(sql).FirstOrDefault();
        }

        /// <summary>
        /// Verifica se o login é válido 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public LoginModel GetLoginData(string login)
        {
            string sql = $@"SELECT USUA_COD = A.USUA_COD
                                  ,USUA_DAT_FIM = A.USUA_DAT_FIM
                                  ,USUA_TXT_SEN = A.USUA_TXT_SEN
                                  ,SN_ACESIG = CONVERT(BIT, CASE
                                                              WHEN (EXISTS (SELECT 1
                                                                            FROM TACE_EMPGRU 
                                                                            WHERE GRPU_COD = A.GRPU_COD
                                                                              AND EMPR_COD = 1
                                                                              AND MODU_COD = 1)) THEN 1
                                                              ELSE 0
                                                            END)
                            FROM TACE_USUARIO A
                            WHERE A.USUA_NOM_LOG = '{login}'";

            return connection.QueryTrim<LoginModel>(sql).FirstOrDefault();
        }

        /// <summary>
        /// Verifica se informada é válida
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordBD"></param>
        /// <returns></returns>
        public bool IsPasswordValid(string password, string passwordBD)
        {
            string sql = $@"SELECT CONVERT(BIT, CASE
                                                  WHEN UPPER(dbo.FGEN_F_DESCRIPTA('{passwordBD}')) = UPPER('{password}') THEN 1
                                                  ELSE 0
                                                END)";

            return connection.QueryTrim<bool>(sql).FirstOrDefault();
        }
    }
}

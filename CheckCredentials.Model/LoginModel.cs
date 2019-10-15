//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe Model do Login
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

using System;

namespace CheckCredentials.Model
{
    public class LoginModel
    {
        public int? USUA_COD { get; set; }
        public DateTime? USUA_DAT_FIM { get; set; }
        public string USUA_TXT_SEN { get; set; }
        public bool SN_ACESIG { get; set; }
    }
}

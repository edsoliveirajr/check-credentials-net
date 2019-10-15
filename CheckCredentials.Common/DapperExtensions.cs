//**********************************************************************************
//  Sistema           : SIGNUS-Scheduler
//  Finalidade        : Classe responnsável por estender os métodos do Dapper
//  Autor             : Edson Oliveira
//  Data da Criação   : 06/02/2019
//
//  ==============================================================================
//
//  Alterado Por      : 
//  Data da Alteração : 
//  Descrição         : 
//
//*********************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dapper
{
    public static class DapperExtensions
    {
        /// <summary>
        /// Função com o objeto de mapear o result do select/procedure do banco de dados na 
        /// classe recebida como parâmetro (semelhante a Query do Dapper),
        /// retirando os espaços no final dos campos string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryTrim<T>(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var dapperResult = SqlMapper.Query<T>(cnn, sql, param, transaction, buffered, commandTimeout, commandType);
            var result = TrimStrings(dapperResult.ToList());
            return result;
        }

        static IEnumerable<T> TrimStrings<T>(IList<T> objects)
        {
            //todo: create an Attribute that can designate that a property shouldn't be trimmed if we need it
            var publicInstanceStringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.PropertyType == typeof(string) && x.CanRead && x.CanWrite);
            foreach (var prop in publicInstanceStringProperties)
            {
                foreach (var obj in objects)
                {
                    var value = (string)prop.GetValue(obj , null);
                    var trimmedValue = value.SafeTrim();
                    prop.SetValue(obj, trimmedValue, null);
                }
            }
            return objects;
        }

        static string SafeTrim(this string source)
        {
            if (source == null)
            {
                return null;
            }
            return source.Trim();
        }
    }

    /// <summary>
    /// Declaração da classe DynamicParameters. Foi alterado a visibilidade do ParamInfo para public
    /// para ter acesso ao propriedades dos parâmetros SQL
    /// </summary>
    partial class DynamicParameters
    {
        // Aumenta a visibilidade da classe ParamInfo
        public partial class ParamInfo { };

        /// <summary>
        /// Função que retorna os parâmetros SQL
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ParamInfo> GetParameters()
        {
            return parameters;
        }
    }
}

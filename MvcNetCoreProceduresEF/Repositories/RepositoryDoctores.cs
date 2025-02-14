using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using NuGet.Protocol;
using System.Data.Common;

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryDoctores
    {
        DoctorContext context;
        public RepositoryDoctores(DoctorContext context)
        {
            this.context = context;
        }
        public async Task<List<Doctor>> GetDoctores()
        {
            string sql = "SP_TODOS_DOCTORES";
            var consulta = await this.context.Doctores.FromSqlRaw(sql).ToListAsync();
            return consulta.ToList();
        }
        public async Task<List<string>> GetEspecialidades()
        {
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_GET_ESPECIALIDADES";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                //  ABRIMOS LA CONEXION A TRAVES DEL COMMAND
                await com.Connection.OpenAsync();
                //  EJECUTAMOS NUESTRO READER
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    string especialidad = reader["ESPECIALIDAD"].ToString();
                    especialidades.Add(especialidad);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }
        public async Task<List<Doctor>> IncrementarSalarioEspecialidad
            (int salario, string especialidad)
        {
            string sql = "SP_UPDATE_SALARIO_ESPECIALIDAD @salario, @especialidad";
            SqlParameter pamSalario = new SqlParameter("@salario", salario);
            SqlParameter pamEspecialidad= new SqlParameter("@especialidad", especialidad);
            var consulta = await this.context.Doctores.FromSqlRaw(sql, pamSalario, pamEspecialidad).ToListAsync();
            return consulta.ToList();
        }
    }
}

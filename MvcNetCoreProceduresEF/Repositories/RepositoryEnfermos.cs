using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Data.Common;
using System.Diagnostics.Metrics;

#region
//create or alter procedure SP_TODOS_ENFERMOS
//as
// select* from ENFERMO
//go
//create or alter procedure SP_FIND_ENFERMO
//(@inscripcion as nvarchar(50))
//as
// select* from ENFERMO where INSCRIPCION=@inscripcion
//go
//create or alter procedure SP_DELETE_ENFERMO
//(@inscripcion as nvarchar(50))
//as
//	delete from ENFERMO where INSCRIPCION=@inscripcion
//go
//create or alter procedure SP_INSERT_ENFERMO
//(@apellido as nvarchar(50), @direccion as nvarchar(50), @fechanac as datetime, @genero as nvarchar(50))
//as
//declare @inscripcion as nvarchar(50)
//declare @nss as nvarchar(50) = '1231231'
//select @inscripcion = MAX(INSCRIPCION)+1 from ENFERMO
//insert into ENFERMO values(@inscripcion, @apellido, @direccion, @fechanac, @genero, @nss);
//go

#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    
    public class RepositoryEnfermos
    {
        HospitalContext context;
        public RepositoryEnfermos(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermos()
        {
            //  PARA CONSULTAS DE SELCCION DEBEMOS MAPEAR
            //  MANUALMENTE LOS DATOS.
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                //  ABRIMOS LA CONEXION A TRAVES DEL COMMAND
                await com.Connection.OpenAsync();
                //  EJECUTAMOS NUESTRO READER
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
            
        }
        public Enfermo FindEnfermo
            (string inscripcion)
        {
            //  PARA LLAMAR A PROCEDIMIENTOS ALMACENADOS
            //  CON PARAMETROS LA LLAMAA SE REALIZA MEDIANTE
            //  EL NOMBRE DE PROCEDIMIENTOS Y CADA PARAMETRO
            //  A CONTINUACION SEPARADO MEDIANTE COMAS
            //  SP_PROCEDIMIENTO @PARAM1, @PARAM2
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            //  DEBEMOS CREAR LOS PARAMETROS
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            //  SI LOS DATOS QUE DEVUELVE EL PROCEDIMIENTO
            //  ESTAN MAPEADOS CON UN MODEL, PODEMOS UTILIZAR
            //  EL METODO FromSqlRaw CON LINQ
            //  CUANDO UTILIZAMOS LINQ CON PROCEMIENTOS ALMACENADOS
            //  LA CONSULTA Y LA ESCTRACCION DE DATOS SE REALIZA EN 
            //  DOS PASOS.
            //  NO SE PUEDE HACER LINQ Y EXTRAER A LA VEZ
            var consulta = this.context.Enfermos.FromSqlRaw(sql, pamInscripcion);
            //  PARA EXTRAER LOS DATOS NECESITAMOS TAMBIEN
            //  EL METODO AsEnumerable()
            Enfermo enfermo =
                consulta.AsEnumerable().FirstOrDefault();
            return enfermo;
        }
        public async Task<Enfermo> FindEnfermoAsync
            (string inscripcion)
        {
            
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            var consulta = await this.context.Enfermos.FromSqlRaw(sql, pamInscripcion).ToListAsync();
            Enfermo enfermo = consulta.FirstOrDefault() ;
            return enfermo;
        }
        public async Task DeleteEnfermo
            (string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamInscripcion);
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }
        public async Task DeleteEnfermoRawAsync
            (string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion = new SqlParameter("@INSCRIPCION", inscripcion);
            //  DENTRO DEL CONTEXT TENEMOS UN METODO PARA
            //  PODER LLAMAR A PROCEDIMIENTOS DE CONSULTAS DE ACCION
            this.context.Database.ExecuteSqlRaw(sql, pamInscripcion);
        }
        public async Task InsertEnfermo
            (string apellido, string direccion, DateTime fechanac, string genero)
        {
            string sql = "SP_INSERT_ENFERMO @apellido, @direccion, @fechanac, @genero";
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamDireccion= new SqlParameter("@direccion", direccion);
            SqlParameter pamFechaNac= new SqlParameter("@fechanac", fechanac);
            SqlParameter pamGenero= new SqlParameter("@genero", genero);
            this.context.Database.ExecuteSqlRaw(sql, pamApellido, pamDireccion, pamFechaNac, pamGenero);
        }
        
    }
}

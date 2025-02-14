using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using NuGet.Configuration;
using System.Diagnostics.Metrics;

namespace MvcNetCoreProceduresEF.Repositories
{
    #region
 //   create or alter view V_WORKERS
 //   as
	//select EMP_NO as IDWORKER
	//, apellido, oficio, salario
 //   from EMP
 //   union

 //   select DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO

 //   FROM DOCTOR

 //   union select EMPLEADO_NO, APELLIDO, FUNCION, SALARIO
 //   from PLANTILLA
 //   go

 //   create or alter procedure SP_WORKERS_OFICIO
 //   (@oficio as nvarchar(50), @personas int out
 //   , @media int out, @suma int out)
 //   as
 //   select* from V_WORKERS
 //   where OFICIO=@oficio
 //   select @personas = COUNT(IDWORKER),
 //   @media = AVG(SALARIO), @suma = SUM(SALARIO)
 //   from V_WORKERS where OFICIO = @oficio
 //   go
    #endregion
    public class RepositoryTrabajadores
    {
        private HospitalContext context;
        public RepositoryTrabajadores(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<TrabajadoresModel> GetTrabajadoresModelAsync()
        {
            var consulta = from datos in this.context.Trabajadores
                           select datos;
            TrabajadoresModel model = new TrabajadoresModel();
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = await consulta.CountAsync();
            model.SumaSalarial = await consulta.SumAsync(z => z.Salario);
            model.MediaSalarial = (int) await consulta.AverageAsync(z => z.Salario);
            return model;
        }
        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Trabajadores
                            select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }
        public async Task<TrabajadoresModel> GetTrabajadoresOficioAsync
            (string oficio)
        {
            //  VAMOSA A LLAMAR AL PROCEDIMIENTO CON EF
            //  LA UNICA DIFERENCIA RADICA EN QUE TENGO QUE 
            //  PONER LA PALABRA out EN CADA PARAMETRO DE SALIDA
            //  EN LA CONSULTA SQL
            string sql = "SP_WORKERS_OFICIO @oficio, @personas OUT, @media OUT, @suma OUT";
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);

            SqlParameter pamPersonas = new SqlParameter("@personas", -1);
            pamPersonas.Direction = System.Data.ParameterDirection.Output;
            SqlParameter pamMedia= new SqlParameter("@media", -1);
            pamMedia.Direction = System.Data.ParameterDirection.Output;
            SqlParameter pamSuma= new SqlParameter("@suma", -1);
            pamSuma.Direction = System.Data.ParameterDirection.Output;
            //  EJECUTAMOS LA CONSULTA DE SELECCION
            var consulta = this.context.Trabajadores.FromSqlRaw
                (sql, pamOficio, pamPersonas, pamMedia, pamSuma);
            TrabajadoresModel model = new TrabajadoresModel();
            //  HASTA QUE NO EXTRAEMOS LOS DATOS DEL SELECT
            //  NO TENEMOS LOS PARAMETROS DE SALIDA(reader.Close())
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = int.Parse(pamPersonas.Value.ToString());
            model.MediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.SumaSalarial = int.Parse(pamSuma.Value.ToString());
            return model;
        }
    }
}

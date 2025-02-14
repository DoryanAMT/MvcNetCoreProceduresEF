using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Diagnostics.Metrics;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MvcNetCoreProceduresEF.Repositories
{
    #region
    //   create or alter view V_EMPLEADOS_DEPARTAMENTOS
    //   as
    //   select CAST(ISNULL(ROW_NUMBER() over (order by APELLIDO), 0) as int) as Id
    //   , EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO
    //   , DEPT.DNOMBRE AS DEPARTAMENTO
    //   , DEPT.LOC AS LOCALIDAD
    //   from EMP
    //   inner join DEPT
    //   on EMP.DEPT_NO = DEPT.DEPT_NO
    //   go
    //create or alter view V_WORKERS
    //as
    // select EMP_NO as IDWORKER
    // , apellido, oficio, salario
    //    from EMP
    //    union

    //    select DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO

    //    FROM DOCTOR

    //    union select EMPLEADO_NO, APELLIDO, FUNCION, SALARIO
    //    from PLANTILLA
    //go

    //create or alter procedure SP_WORKERS_OFICIO
    //(@oficio as nvarchar(50), @personas int out
    //, @media int out, @suma int out)
    //as
    //select* from V_WORKERS
    //where OFICIO=@oficio
    //select @personas = COUNT(IDWORKER),
    //@media = AVG(SALARIO), @suma = SUM(SALARIO)
    //from V_WORKERS where OFICIO = @oficio
    //go
    //create or alter view V_EMPLEADOS_DEPARTAMENTOS
    //as
	   // select CAST(ISNULL(ROW_NUMBER() over (order by APELLIDO), 0) as int) as Id
	   // , EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO
	   // , DEPT.DNOMBRE AS DEPARTAMENTO
	   // , DEPT.LOC AS LOCALIDAD
    //    from EMP
    //    inner join DEPT

    //    on EMP.DEPT_NO = DEPT.DEPT_NO
    //go
    #endregion
    public class RepositoryEmpleados
    {
        HospitalContext context;
        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<VistaEmpleado>> GetVistaEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistaEmpleado
                           select datos;
            return await consulta.ToListAsync();
        }
    }
}

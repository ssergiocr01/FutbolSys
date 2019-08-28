using FutbolSys.Web.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FutbolSys.Web.Helpers
{
    public class DBHelper
    {
        public static async Task<Response> SaveChanges(DataContextLocal db)
        {
            try
            {
                await db.SaveChangesAsync();
                return new Response { Succeeded = true, };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false, };

                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Hay un registro con el mismo valor.";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "El registro no se puede eliminar porque tiene registros relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }
    }
}
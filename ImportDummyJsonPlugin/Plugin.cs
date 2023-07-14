using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using Newtonsoft.Json;

namespace ImportDummyJsonPlugin
{
    [Author(Name = "Nikolai Solomatov")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private const string ROUTE = "https://dummyjson.com/users";

        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Starting ImportJsonPlugin");

            var employeesList = args.Cast<EmployeesDTO>().ToList();

            using (var client = new HttpClient())
            {
                try
                {
                    var result = client.GetAsync(ROUTE + "?limit=5").GetAwaiter().GetResult();
                    var responseUsersDto =
                        JsonConvert.DeserializeObject<ResponseUsersDto>(result.Content.ReadAsStringAsync().GetAwaiter()
                            .GetResult());

                    employeesList.AddRange(responseUsersDto.Users);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }

            return employeesList.Cast<DataTransferObject>();
        }
    }
}
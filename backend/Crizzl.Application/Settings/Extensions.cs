using System;
using Crizzl.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Crizzl.Application.Settings
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse httpResponse, string message)
        {
            httpResponse.Headers.Add("Application-Error", message);
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int GetAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > DateTime.Today) age--;

            return age;
        }

        public static void AddPagination(this HttpResponse httpResponse, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginatedResult = new PaginatedResult(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            httpResponse.Headers.Add("Pagination", JsonConvert.SerializeObject(paginatedResult, camelCaseFormatter));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
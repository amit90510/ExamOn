using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer
{
    public static class JsonResponse
    {
        public static JsonData JsonResponseData(JsonData json)
        {
            return new JsonData() {
                StatusCode = json.StatusCode,
                Error = json.Error,
                Data = json.Data
            };   
        }
    }

    public class JsonData
    {
        public int StatusCode { get; set; }

        public string Error { get; set; }

        public object Data { get; set; }
    }
}
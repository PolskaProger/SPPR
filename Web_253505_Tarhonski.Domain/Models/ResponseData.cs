﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_253505_Tarhonski.Domain.Models
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public bool Successfull { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T> { Data = data };
        }
        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Successfull = false,
                Data = data
            };
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class BaseResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }

    public class Response
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class RequesterInfo
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string AccessToken { get; set; }
    }
}

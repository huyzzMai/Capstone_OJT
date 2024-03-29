﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Payload.ResponseModel.NotificationResponse
{
    public class NotificationResponse
    {
        public int Id {  get; set; }    
        public string Title { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }   
        public bool IsRead { get; set; }
    }
}

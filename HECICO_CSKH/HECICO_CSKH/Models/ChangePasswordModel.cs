﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HECICO_CSKH.Models
{
   public class ChangePasswordModel
    {
        public string Username { get; set; } 
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

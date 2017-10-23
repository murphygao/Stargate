﻿using Aiursoft.Pylon;
using Aiursoft.Pylon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Controllers
{
    [AiurRequireHttps]
    [AiurExceptionHandler]
    public class ApiController : AiurApiController
    {
    }
}

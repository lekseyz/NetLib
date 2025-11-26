using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Domain.Dtos
{
    public class ChangeClientDto
    {
        public string Name { get; set; } = null;
        public string PassportId { get; set; } = null;
    }
}
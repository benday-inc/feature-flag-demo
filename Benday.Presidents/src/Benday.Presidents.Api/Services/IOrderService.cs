using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presidents.Api.Services
{
    public interface IOrderService
    {
        Order GetById(int orderId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Guid GetCurrentUserId();
    }
}

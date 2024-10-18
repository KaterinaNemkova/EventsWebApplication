using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Core.Abstractions
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T instance);
    }
}

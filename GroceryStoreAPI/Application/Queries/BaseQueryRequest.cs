using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Application.Queries
{
    public class BaseQueryRequest<T>: IRequest<T>
    {
    }
}

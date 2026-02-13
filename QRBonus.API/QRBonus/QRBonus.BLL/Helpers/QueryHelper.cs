using Microsoft.EntityFrameworkCore;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable<T> IgnoreExceptDeleted<T>(this IQueryable<T> query)
            where T : BaseEntity
        {
            return query.IgnoreQueryFilters().Where(x => !x.IsDeleted);
        }
    }
}

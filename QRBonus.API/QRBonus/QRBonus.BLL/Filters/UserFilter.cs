using InnLine.BLL.Filters;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using QRBonus.BLL.Enums;
using QRBonus.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QRBonus.BLL.Filters
{
    public class UserFilter : BaseFilter<User>
    {
        public long? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? OrderBy { get; set; } = null;


        public override IQueryable<User> CreateQuery(IQueryable<User> query)
        {
            if (Query != null)
            {
                return Query;
            }

            if (Id.HasValue)
            {
                query = query.Where(x => x.Id == Id);
            }

            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                query = query.Where(x => x.FirstName.ToLower().Replace(" ", "").Contains(FirstName.ToLower().Replace(" ", "")));
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                query = query.Where(x => x.LastName.ToLower().Replace(" ", "").Contains(LastName.ToLower().Replace(" ", "")));
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                query = query.Where(x => x.UserName.ToLower().Replace(" ", "").Contains(UserName.ToLower().Replace(" ", "")));
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                return OrderBy.ToLower() switch
                {
                    OrderByConst.Id =>  query.OrderBy(x => x.Id),
                    OrderByConst.IdDesc => query.OrderByDescending(x => x.Id),
                    OrderByConst.FirstName => query.OrderBy(x => x.FirstName),
                    OrderByConst.FirstNameDesc => query.OrderByDescending(x => x.FirstName),
                    OrderByConst.LastName => query.OrderBy(x => x.LastName),
                    OrderByConst.LastNameDesc => query.OrderByDescending(x => x.LastName),
                    OrderByConst.UserName => query.OrderBy(x => x.UserName),
                    OrderByConst.UserNameDesc => query.OrderByDescending(x => x.UserName),
                    _ => query.OrderByDescending(x => x.CreatedDate),
                };
            }

            return query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }
    }

}
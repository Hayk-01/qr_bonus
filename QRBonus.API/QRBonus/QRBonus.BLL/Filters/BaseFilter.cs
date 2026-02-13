using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnLine.BLL.Filters;
public abstract class BaseFilter<T>
{
    private int skip = 1;
    public int Skip
    {
        get
        {
            return skip;
        }
        set
        {
            skip = value < 1 ? 1 : value;
        }
    }

    public bool? IsDeleted { get; set; }
    public int? Take { get; set; }

    protected IQueryable<T> Query { get; set; }

    public abstract IQueryable<T> CreateQuery(IQueryable<T> query);

    public async Task<int> CountAsync(IQueryable<T> query)
    {
        query = CreateQuery(query);
        return await query.CountAsync();
    }

    public int Count(IQueryable<T> query)
    {
        query = CreateQuery(query);
        return query.Count();
    }

    public decimal Sum(IQueryable<T> query, Expression<Func<T, decimal>> func)
    {
        query = CreateQuery(query);
        return query.Sum(func);
    }

    public long Sum(IQueryable<T> query, Expression<Func<T, long>> func)
    {
        query = CreateQuery(query);
        return query.Sum(func);
    }

    public async Task<decimal> SumAsync(IQueryable<T> query, Expression<Func<T, decimal>> func)
    {
        query = CreateQuery(query);
        return await query.SumAsync(func);
    }

    public async Task<long> SumAsync(IQueryable<T> query, Expression<Func<T, long>> func)
    {
        query = CreateQuery(query);
        return await query.SumAsync(func);
    }

    public IQueryable<T> FilterObjects(IQueryable<T> query)
    {
        query = CreateQuery(query);

        if (Query == null)
            Query = query;
        if (Take.HasValue)
            query = query.Skip((Skip - 1) * Take.Value).Take(Take.Value);

        return query;
    }

    public PagedInfo GetPagedInfo(int count)
    {
        var totalPages = Take.HasValue ? (int)Math.Ceiling((decimal)count / (decimal)Take.Value) : 1;

        return new PagedInfo(Skip, Take ?? count, totalPages, count);
    }

    public PagedInfo GetPagedInfo(IQueryable<T> query)
    {
        var count = Count(query);

        var totalPages = Take.HasValue ? (int)Math.Ceiling((decimal)count / (decimal)Take.Value) : 1;

        return new PagedInfo(Skip, Take ?? count, totalPages, count);
    }

    public async Task<PagedInfo> GetPagedInfoAsync(IQueryable<T> query)
    {
        var count = await CountAsync(query);

        var totalPages = Take.HasValue ? (int)Math.Ceiling((decimal)count / (decimal)Take.Value) : 1;

        return new PagedInfo(Skip, Take ?? count, totalPages, count);
    }
}

using System.Linq.Expressions;

using Application.Data;
using Application.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected LibraryContext LibraryContext { get; set; }

    protected RepositoryBase(LibraryContext libraryContext)
    {
        LibraryContext = libraryContext;
    }

    public IQueryable<T> FindAll()
    {
        return LibraryContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return LibraryContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    {
        LibraryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        LibraryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        LibraryContext.Set<T>().Remove(entity);
    }
}
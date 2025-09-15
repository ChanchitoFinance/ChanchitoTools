using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace ChanchitoBackend.System.Abstractions.DataTesting
{
    /// <summary>
    /// Implementation of data mock builder for comprehensive data layer testing
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type</typeparam>
    public class DataMockBuilder<TDbContext> : IDataMockBuilder<TDbContext> where TDbContext : DbContext
    {
        private readonly Mock<TDbContext> _mockDbContext;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Dictionary<Type, object> _mockDbSets;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMockBuilder{TDbContext}"/> class
        /// </summary>
        public DataMockBuilder()
        {
            _mockDbContext = new Mock<TDbContext>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockDbSets = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Gets the mocked DbContext
        /// </summary>
        public Mock<TDbContext> MockDbContext => _mockDbContext;

        /// <summary>
        /// Gets the mocked service provider
        /// </summary>
        public Mock<IServiceProvider> MockServiceProvider => _mockServiceProvider;

        /// <summary>
        /// Mocks a DbSet for the specified entity type
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="data">The data to include in the mock</param>
        /// <returns>The mock DbSet</returns>
        public Mock<DbSet<TEntity>> MockDbSet<TEntity>(IEnumerable<TEntity>? data = null) where TEntity : class
        {
            var mockDbSet = new Mock<DbSet<TEntity>>();
            var queryableData = (data ?? Enumerable.Empty<TEntity>()).AsQueryable();

            // Setup IQueryable
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            // Setup IAsyncEnumerable
            mockDbSet.As<IAsyncEnumerable<TEntity>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<TEntity>(queryableData.GetEnumerator()));

            // Setup IQueryable<TEntity>
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<TEntity>(queryableData.Provider));

            // Setup DbSet methods
            mockDbSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>(entity => { });
            mockDbSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>(entities => { });
            mockDbSet.Setup(d => d.Remove(It.IsAny<TEntity>())).Callback<TEntity>(entity => { });
            mockDbSet.Setup(d => d.RemoveRange(It.IsAny<IEnumerable<TEntity>>())).Callback<IEnumerable<TEntity>>(entities => { });

            // Store the mock for later use
            _mockDbSets[typeof(TEntity)] = mockDbSet;

            return mockDbSet;
        }

        /// <summary>
        /// Mocks a DbSet for the specified entity type with async support
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="data">The data to include in the mock</param>
        /// <returns>The mock DbSet</returns>
        public Mock<DbSet<TEntity>> MockDbSetAsync<TEntity>(IEnumerable<TEntity>? data = null) where TEntity : class
        {
            var mockDbSet = MockDbSet<TEntity>(data);
            var queryableData = (data ?? Enumerable.Empty<TEntity>()).AsQueryable();

            // Setup async methods
            mockDbSet.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryableData.ToList());

            mockDbSet.Setup(d => d.FirstOrDefaultAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<TEntity, bool>> predicate, CancellationToken token) =>
                    queryableData.FirstOrDefault(predicate.Compile()));

            mockDbSet.Setup(d => d.CountAsync(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<TEntity, bool>> predicate, CancellationToken token) =>
                    queryableData.Count(predicate.Compile()));

            return mockDbSet;
        }

        /// <summary>
        /// Mocks a service
        /// </summary>
        /// <typeparam name="TService">The service type</typeparam>
        /// <returns>The mock service</returns>
        public Mock<TService> MockService<TService>() where TService : class
        {
            var mockService = new Mock<TService>();

            // Setup service provider to return the mock service
            _mockServiceProvider.Setup(sp => sp.GetService(typeof(TService)))
                .Returns(mockService.Object);

            return mockService;
        }

        /// <summary>
        /// Builds the complete mock setup
        /// </summary>
        /// <returns>The configured mock DbContext</returns>
        public Mock<TDbContext> Build()
        {
            // Setup Set<T>() method to return appropriate mock DbSet
            _mockDbContext.Setup(d => d.Set<It.IsAnyType>())
                .Returns((Type entityType) =>
                {
                    if (_mockDbSets.TryGetValue(entityType, out var mockDbSet))
                    {
                        return mockDbSet;
                    }
                    throw new InvalidOperationException($"No mock DbSet configured for entity type {entityType.Name}");
                });

            // Setup SaveChanges
            _mockDbContext.Setup(d => d.SaveChanges()).Returns(1);
            _mockDbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Setup Database property
            var mockDatabase = new Mock<DatabaseFacade>(_mockDbContext.Object);
            _mockDbContext.Setup(d => d.Database).Returns(mockDatabase.Object);

            return _mockDbContext;
        }
    }

    /// <summary>
    /// Test async enumerator for mocking async operations
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }

    /// <summary>
    /// Test async query provider for mocking async operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            // For non-generic queries, return a simple enumerable
            return Enumerable.Empty<object>().AsQueryable();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            // For generic queries, return an empty enumerable of the correct type
            return Enumerable.Empty<TElement>().AsQueryable();
        }

        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var resultType = typeof(TResult);
            var result = _inner.Execute(expression);

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ValueTask<>))
            {
                var valueTaskType = resultType.GetGenericArguments()[0];
                var valueTask = Activator.CreateInstance(resultType, Task.FromResult(result));
                return (TResult)valueTask;
            }

            return (TResult)result;
        }
    }

    /// <summary>
    /// Test async enumerable for mocking async operations
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryProvider _provider;

        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
            _provider = new TestAsyncQueryProvider<T>(enumerable.AsQueryable().Provider);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => _provider;
    }
}
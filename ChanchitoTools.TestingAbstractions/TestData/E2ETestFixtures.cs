using ChanchitoBackend.System.Abstractions.Builders;
using Microsoft.EntityFrameworkCore;

namespace ChanchitoBackend.System.Abstractions.TestData
{
    /// <summary>
    /// E2E test fixtures that provide pre-configured test data and scenarios
    /// </summary>
    public class E2ETestFixtures
    {
        /// <summary>
        /// Gets the test database context
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// Gets the test user data
        /// </summary>
        public TestUserData TestUser { get; private set; } = null!;

        /// <summary>
        /// Gets the test categories
        /// </summary>
        public TestCategoryData TestCategories { get; private set; } = null!;

        /// <summary>
        /// Gets the test products
        /// </summary>
        public TestProductData TestProducts { get; private set; } = null!;

        /// <summary>
        /// Gets the test expenses
        /// </summary>
        public TestExpenseData TestExpenses { get; private set; } = null!;

        public E2ETestFixtures(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Initializes the test fixtures with test data
        /// </summary>
        public async Task InitializeAsync()
        {
            await CreateTestUserAsync();
            await CreateTestCategoriesAsync();
            await CreateTestProductsAsync();
            await CreateTestExpensesAsync();
        }

        /// <summary>
        /// Creates a test user
        /// </summary>
        private async Task CreateTestUserAsync()
        {
            // This would typically create a user in the database
            // For now, we'll create mock data
            TestUser = new TestUserData
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Test User",
                Email = "test@example.com",
                Password = "TestPassword123!"
            };
        }

        /// <summary>
        /// Creates test categories
        /// </summary>
        private async Task CreateTestCategoriesAsync()
        {
            TestCategories = new TestCategoryData
            {
                Food = new TestCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Food",
                    OwnerType = "system",
                    OwnerId = null,
                    ParentId = null
                },
                Transportation = new TestCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Transportation",
                    OwnerType = "system",
                    OwnerId = null,
                    ParentId = null
                },
                Utilities = new TestCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Utilities",
                    OwnerType = "system",
                    OwnerId = null,
                    ParentId = null
                },
                PersonalCare = new TestCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Personal Care",
                    OwnerType = "system",
                    OwnerId = null,
                    ParentId = null
                },
                UserCategory = new TestCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Personal Category",
                    OwnerType = "user",
                    OwnerId = TestUser.Id,
                    ParentId = null
                }
            };
        }

        /// <summary>
        /// Creates test products
        /// </summary>
        private async Task CreateTestProductsAsync()
        {
            TestProducts = new TestProductData
            {
                Bread = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Bread",
                    CategoryId = TestCategories.Food.Id,
                    OwnerType = "system",
                    OwnerId = null,
                    Notes = "Fresh bread"
                },
                Milk = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Milk",
                    CategoryId = TestCategories.Food.Id,
                    OwnerType = "system",
                    OwnerId = null,
                    Notes = "Fresh milk"
                },
                BusFare = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Bus Fare",
                    CategoryId = TestCategories.Transportation.Id,
                    OwnerType = "system",
                    OwnerId = null,
                    Notes = "Public transportation fare"
                },
                Shampoo = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Shampoo",
                    CategoryId = TestCategories.PersonalCare.Id,
                    OwnerType = "system",
                    OwnerId = null,
                    Notes = "Hair care product"
                },
                Electricity = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Electricity",
                    CategoryId = TestCategories.Utilities.Id,
                    OwnerType = "system",
                    OwnerId = null,
                    Notes = "Electricity bill"
                },
                UserProduct = new TestProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Personal Product",
                    CategoryId = TestCategories.UserCategory.Id,
                    OwnerType = "user",
                    OwnerId = TestUser.Id,
                    Notes = "User-specific product"
                }
            };
        }

        /// <summary>
        /// Creates test expenses
        /// </summary>
        private async Task CreateTestExpensesAsync()
        {
            TestExpenses = new TestExpenseData
            {
                GroceryShopping = new TestExpense
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = TestUser.Id,
                    TransactionDate = DateTime.UtcNow,
                    Currency = "BOB",
                    Notes = "Weekly grocery shopping",
                    Items = new List<TestExpenseItem>
                    {
                        new TestExpenseItem
                        {
                            ProductId = TestProducts.Bread.Id,
                            Quantity = "2.000",
                            UnitPrice = "8.5000",
                            Notes = "Fresh bread"
                        },
                        new TestExpenseItem
                        {
                            ProductId = TestProducts.Milk.Id,
                            Quantity = "1.000",
                            UnitPrice = "12.0000",
                            Notes = "Fresh milk"
                        }
                    }
                },
                Transportation = new TestExpense
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = TestUser.Id,
                    TransactionDate = DateTime.UtcNow,
                    Currency = "BOB",
                    Notes = "Daily commute",
                    Items = new List<TestExpenseItem>
                    {
                        new TestExpenseItem
                        {
                            ProductId = TestProducts.BusFare.Id,
                            Quantity = "1.000",
                            UnitPrice = "3.5000",
                            Notes = "Bus fare"
                        }
                    }
                },
                PersonalCare = new TestExpense
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = TestUser.Id,
                    TransactionDate = DateTime.UtcNow,
                    Currency = "BOB",
                    Notes = "Personal care items",
                    Items = new List<TestExpenseItem>
                    {
                        new TestExpenseItem
                        {
                            ProductId = TestProducts.Shampoo.Id,
                            Quantity = "1.000",
                            UnitPrice = "25.0000",
                            Notes = "Shampoo"
                        }
                    }
                },
                Utilities = new TestExpense
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = TestUser.Id,
                    TransactionDate = DateTime.UtcNow,
                    Currency = "BOB",
                    Notes = "Monthly utilities",
                    Items = new List<TestExpenseItem>
                    {
                        new TestExpenseItem
                        {
                            ProductId = TestProducts.Electricity.Id,
                            Quantity = "1.000",
                            UnitPrice = "150.0000",
                            Notes = "Electricity bill"
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Gets test data builders for creating additional test data
        /// </summary>
        public object Builders => new object(); // Placeholder - builders are in E2ETestDataBuilders

        /// <summary>
        /// Creates a complete test scenario
        /// </summary>
        public async Task<CompleteTestScenario> CreateCompleteScenarioAsync()
        {
            await InitializeAsync();
            return new CompleteTestScenario(this);
        }

        /// <summary>
        /// Cleans up test data
        /// </summary>
        public async Task CleanupAsync()
        {
            // This would typically clean up the database
            // For now, we'll just clear the references
            TestUser = null!;
            TestCategories = null!;
            TestProducts = null!;
            TestExpenses = null!;
        }
    }

    /// <summary>
    /// Complete test scenario with all test data
    /// </summary>
    public class CompleteTestScenario
    {
        public E2ETestFixtures Fixtures { get; }
        public TestUserData User => Fixtures.TestUser;
        public TestCategoryData Categories => Fixtures.TestCategories;
        public TestProductData Products => Fixtures.TestProducts;
        public TestExpenseData Expenses => Fixtures.TestExpenses;

        public CompleteTestScenario(E2ETestFixtures fixtures)
        {
            Fixtures = fixtures;
        }

        /// <summary>
        /// Gets all test data as a dictionary for easy access
        /// </summary>
        public Dictionary<string, object> GetAllTestData()
        {
            return new Dictionary<string, object>
            {
                ["User"] = User,
                ["Categories"] = Categories,
                ["Products"] = Products,
                ["Expenses"] = Expenses
            };
        }

        /// <summary>
        /// Gets test data by type
        /// </summary>
        public T GetTestData<T>(string key) where T : class
        {
            var data = GetAllTestData();
            return data[key] as T ?? throw new KeyNotFoundException($"Test data with key '{key}' not found");
        }
    }

    #region Test Data Models

    /// <summary>
    /// Test user data
    /// </summary>
    public class TestUserData
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Test category data
    /// </summary>
    public class TestCategory
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OwnerType { get; set; } = string.Empty;
        public string? OwnerId { get; set; }
        public string? ParentId { get; set; }
    }

    /// <summary>
    /// Test product data
    /// </summary>
    public class TestProduct
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string OwnerType { get; set; } = string.Empty;
        public string? OwnerId { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Test expense data
    /// </summary>
    public class TestExpense
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<TestExpenseItem> Items { get; set; } = new();
    }

    /// <summary>
    /// Test expense item data
    /// </summary>
    public class TestExpenseItem
    {
        public string ProductId { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string UnitPrice { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Test categories collection
    /// </summary>
    public class TestCategoryData
    {
        public TestCategory Food { get; set; } = null!;
        public TestCategory Transportation { get; set; } = null!;
        public TestCategory Utilities { get; set; } = null!;
        public TestCategory PersonalCare { get; set; } = null!;
        public TestCategory UserCategory { get; set; } = null!;
    }

    /// <summary>
    /// Test products collection
    /// </summary>
    public class TestProductData
    {
        public TestProduct Bread { get; set; } = null!;
        public TestProduct Milk { get; set; } = null!;
        public TestProduct BusFare { get; set; } = null!;
        public TestProduct Shampoo { get; set; } = null!;
        public TestProduct Electricity { get; set; } = null!;
        public TestProduct UserProduct { get; set; } = null!;
    }

    /// <summary>
    /// Test expenses collection
    /// </summary>
    public class TestExpenseData
    {
        public TestExpense GroceryShopping { get; set; } = null!;
        public TestExpense Transportation { get; set; } = null!;
        public TestExpense PersonalCare { get; set; } = null!;
        public TestExpense Utilities { get; set; } = null!;
    }

    #endregion
}

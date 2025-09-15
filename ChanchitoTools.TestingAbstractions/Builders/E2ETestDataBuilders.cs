using System.Text.Json;

namespace ChanchitoBackend.System.Abstractions.Builders
{
    /// <summary>
    /// E2E test data builders for creating test data according to the data contract specifications
    /// </summary>
    public class E2ETestDataBuilders
    {
        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static E2ETestDataBuilders Instance => new();

        /// <summary>
        /// Creates a test user registration request
        /// </summary>
        public static object CreateUserRegistrationRequest(
            string? fullName = null,
            string? email = null,
            string? password = null)
        {
            return new
            {
                fullName = fullName ?? "Test User",
                email = email ?? $"test{Guid.NewGuid():N}@example.com",
                password = password ?? "TestPassword123!"
            };
        }

        /// <summary>
        /// Creates a test user login request
        /// </summary>
        public static object CreateUserLoginRequest(
            string? email = null,
            string? password = null)
        {
            return new
            {
                email = email ?? "test@example.com",
                password = password ?? "TestPassword123!"
            };
        }

        /// <summary>
        /// Creates a test category creation request
        /// </summary>
        public static object CreateCategoryRequest(
            string? name = null,
            string? ownerType = null,
            string? ownerId = null,
            string? parentId = null)
        {
            return new
            {
                name = name ?? "Test Category",
                ownerType = ownerType ?? "user",
                ownerId = ownerId,
                parentId = parentId
            };
        }

        /// <summary>
        /// Creates a test category update request
        /// </summary>
        public static object CreateCategoryUpdateRequest(
            string? name = null,
            string? parentId = null)
        {
            return new
            {
                name = name ?? "Updated Category",
                parentId = parentId
            };
        }

        /// <summary>
        /// Creates a test product creation request
        /// </summary>
        public static object CreateProductRequest(
            string? name = null,
            string? categoryId = null,
            string? ownerType = null,
            string? ownerId = null,
            string? notes = null)
        {
            return new
            {
                name = name ?? "Test Product",
                categoryId = categoryId ?? Guid.NewGuid().ToString(),
                ownerType = ownerType ?? "user",
                ownerId = ownerId,
                notes = notes
            };
        }

        /// <summary>
        /// Creates a test product update request
        /// </summary>
        public static object CreateProductUpdateRequest(
            string? name = null,
            string? categoryId = null,
            string? notes = null)
        {
            return new
            {
                name = name ?? "Updated Product",
                categoryId = categoryId,
                notes = notes
            };
        }

        /// <summary>
        /// Creates a test expense creation request
        /// </summary>
        public static object CreateExpenseRequest(
            DateTime? transactionDate = null,
            string? currency = null,
            string? notes = null,
            List<object>? items = null)
        {
            return new
            {
                transactionDate = transactionDate ?? DateTime.UtcNow,
                currency = currency ?? "BOB",
                notes = notes ?? "Test expense",
                items = items ?? new List<object>
                {
                    CreateExpenseItemRequest()
                }
            };
        }

        /// <summary>
        /// Creates a test expense item request
        /// </summary>
        public static object CreateExpenseItemRequest(
            string? productId = null,
            string? quantity = null,
            string? unitPrice = null,
            string? notes = null)
        {
            return new
            {
                productId = productId ?? Guid.NewGuid().ToString(),
                quantity = quantity ?? "1.000",
                unitPrice = unitPrice ?? "19.9900",
                notes = notes
            };
        }

        /// <summary>
        /// Creates a test expense update request
        /// </summary>
        public static object CreateExpenseUpdateRequest(
            DateTime? transactionDate = null,
            string? currency = null,
            string? notes = null,
            List<object>? items = null)
        {
            return new
            {
                transactionDate = transactionDate ?? DateTime.UtcNow,
                currency = currency ?? "BOB",
                notes = notes ?? "Updated expense",
                items = items
            };
        }

        /// <summary>
        /// Creates test categories for different scenarios
        /// </summary>
        public static class Categories
        {
            /// <summary>
            /// Creates a food category
            /// </summary>
            public static object Food => CreateCategoryRequest("Food", "system");

            /// <summary>
            /// Creates a transportation category
            /// </summary>
            public static object Transportation => CreateCategoryRequest("Transportation", "system");

            /// <summary>
            /// Creates a utilities category
            /// </summary>
            public static object Utilities => CreateCategoryRequest("Utilities", "system");

            /// <summary>
            /// Creates a personal care category
            /// </summary>
            public static object PersonalCare => CreateCategoryRequest("Personal Care", "system");

            /// <summary>
            /// Creates a user-specific category
            /// </summary>
            public static object CreateUserCategory(string ownerId, string name = "Personal Category")
            {
                return CreateCategoryRequest(name, "user", ownerId);
            }
        }

        /// <summary>
        /// Creates test products for different scenarios
        /// </summary>
        public static class Products
        {
            /// <summary>
            /// Creates a bread product
            /// </summary>
            public static object Bread(string categoryId) => CreateProductRequest("Bread", categoryId, "system");

            /// <summary>
            /// Creates a milk product
            /// </summary>
            public static object Milk(string categoryId) => CreateProductRequest("Milk", categoryId, "system");

            /// <summary>
            /// Creates a bus fare product
            /// </summary>
            public static object BusFare(string categoryId) => CreateProductRequest("Bus Fare", categoryId, "system");

            /// <summary>
            /// Creates a user-specific product
            /// </summary>
            public static object CreateUserProduct(string categoryId, string ownerId, string name = "Personal Product")
            {
                return CreateProductRequest(name, categoryId, "user", ownerId);
            }
        }

        /// <summary>
        /// Creates test expenses for different scenarios
        /// </summary>
        public static class Expenses
        {
            /// <summary>
            /// Creates a grocery shopping expense
            /// </summary>
            public static object GroceryShopping(string productId, string quantity = "2.000", string unitPrice = "15.5000")
            {
                return CreateExpenseRequest(
                    DateTime.UtcNow,
                    "BOB",
                    "Grocery shopping",
                    new List<object>
                    {
                        CreateExpenseItemRequest(productId, quantity, unitPrice, "Fresh groceries")
                    });
            }

            /// <summary>
            /// Creates a transportation expense
            /// </summary>
            public static object Transportation(string productId, string quantity = "1.000", string unitPrice = "3.5000")
            {
                return CreateExpenseRequest(
                    DateTime.UtcNow,
                    "BOB",
                    "Daily commute",
                    new List<object>
                    {
                        CreateExpenseItemRequest(productId, quantity, unitPrice, "Bus fare")
                    });
            }

            /// <summary>
            /// Creates a multi-item expense
            /// </summary>
            public static object MultiItemExpense(List<string> productIds, string currency = "BOB")
            {
                var items = productIds.Select(id => CreateExpenseItemRequest(id, "1.000", "10.0000")).ToList();

                return CreateExpenseRequest(
                    DateTime.UtcNow,
                    currency,
                    "Multiple items purchase",
                    items);
            }
        }

        /// <summary>
        /// Creates test currencies
        /// </summary>
        public static class Currencies
        {
            /// <summary>
            /// Bolivian Boliviano
            /// </summary>
            public const string BOB = "BOB";

            /// <summary>
            /// US Dollar
            /// </summary>
            public const string USD = "USD";

            /// <summary>
            /// Euro
            /// </summary>
            public const string EUR = "EUR";

            /// <summary>
            /// Gets all supported currencies
            /// </summary>
            public static string[] All => new[] { BOB, USD, EUR };
        }

        /// <summary>
        /// Creates test quantities
        /// </summary>
        public static class Quantities
        {
            /// <summary>
            /// Single unit
            /// </summary>
            public const string One = "1.000";

            /// <summary>
            /// Half unit
            /// </summary>
            public const string Half = "0.500";

            /// <summary>
            /// Quarter unit
            /// </summary>
            public const string Quarter = "0.250";

            /// <summary>
            /// Creates a custom quantity
            /// </summary>
            public static string Custom(decimal value)
            {
                return value.ToString("F3");
            }
        }

        /// <summary>
        /// Creates test money values
        /// </summary>
        public static class Money
        {
            /// <summary>
            /// Zero amount
            /// </summary>
            public const string Zero = "0.0000";

            /// <summary>
            /// Small amount
            /// </summary>
            public const string Small = "1.0000";

            /// <summary>
            /// Medium amount
            /// </summary>
            public const string Medium = "10.0000";

            /// <summary>
            /// Large amount
            /// </summary>
            public const string Large = "100.0000";

            /// <summary>
            /// Creates a custom money amount
            /// </summary>
            public static string Custom(decimal value)
            {
                return value.ToString("F4");
            }
        }

        /// <summary>
        /// Creates test dates
        /// </summary>
        public static class Dates
        {
            /// <summary>
            /// Today
            /// </summary>
            public static DateTime Today => DateTime.UtcNow.Date;

            /// <summary>
            /// Yesterday
            /// </summary>
            public static DateTime Yesterday => DateTime.UtcNow.Date.AddDays(-1);

            /// <summary>
            /// Last week
            /// </summary>
            public static DateTime LastWeek => DateTime.UtcNow.Date.AddDays(-7);

            /// <summary>
            /// Last month
            /// </summary>
            public static DateTime LastMonth => DateTime.UtcNow.Date.AddMonths(-1);

            /// <summary>
            /// Creates a custom date
            /// </summary>
            public static DateTime Custom(int year, int month, int day)
            {
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Creates test user data
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// Creates a test user with default values
            /// </summary>
            public static object Default => CreateUserRegistrationRequest();

            /// <summary>
            /// Creates a test user with custom values
            /// </summary>
            public static object Custom(string fullName, string email, string password)
            {
                return CreateUserRegistrationRequest(fullName, email, password);
            }

            /// <summary>
            /// Creates multiple test users
            /// </summary>
            public static List<object> Multiple(int count)
            {
                var users = new List<object>();
                for (int i = 0; i < count; i++)
                {
                    users.Add(CreateUserRegistrationRequest(
                        $"Test User {i + 1}",
                        $"test{i + 1}@example.com",
                        $"TestPassword{i + 1}!"
                    ));
                }
                return users;
            }
        }

        /// <summary>
        /// Creates test data sets for different scenarios
        /// </summary>
        public static class TestScenarios
        {
            /// <summary>
            /// Creates a complete grocery shopping scenario
            /// </summary>
            public static GroceryShoppingScenario GroceryShopping()
            {
                return new GroceryShoppingScenario();
            }

            /// <summary>
            /// Creates a complete transportation scenario
            /// </summary>
            public static TransportationScenario Transportation()
            {
                return new TransportationScenario();
            }

            /// <summary>
            /// Creates a complete personal finance scenario
            /// </summary>
            public static PersonalFinanceScenario PersonalFinance()
            {
                return new PersonalFinanceScenario();
            }
        }
    }

    /// <summary>
    /// Grocery shopping test scenario
    /// </summary>
    public class GroceryShoppingScenario
    {
        public object FoodCategory { get; }
        public object BreadProduct { get; }
        public object MilkProduct { get; }
        public object GroceryExpense { get; }

        public GroceryShoppingScenario()
        {
            var categoryId = Guid.NewGuid().ToString();
            FoodCategory = E2ETestDataBuilders.Categories.Food;
            BreadProduct = E2ETestDataBuilders.Products.Bread(categoryId);
            MilkProduct = E2ETestDataBuilders.Products.Milk(categoryId);

            var items = new List<object>
            {
                E2ETestDataBuilders.CreateExpenseItemRequest(categoryId, "2.000", "8.5000", "Fresh bread"),
                E2ETestDataBuilders.CreateExpenseItemRequest(categoryId, "1.000", "12.0000", "Fresh milk")
            };

            GroceryExpense = E2ETestDataBuilders.CreateExpenseRequest(
                DateTime.UtcNow,
                "BOB",
                "Weekly grocery shopping",
                items);
        }
    }

    /// <summary>
    /// Transportation test scenario
    /// </summary>
    public class TransportationScenario
    {
        public object TransportationCategory { get; }
        public object BusFareProduct { get; }
        public object TransportationExpense { get; }

        public TransportationScenario()
        {
            var categoryId = Guid.NewGuid().ToString();
            TransportationCategory = E2ETestDataBuilders.Categories.Transportation;
            BusFareProduct = E2ETestDataBuilders.Products.BusFare(categoryId);

            TransportationExpense = E2ETestDataBuilders.Expenses.Transportation(categoryId);
        }
    }

    /// <summary>
    /// Personal finance test scenario
    /// </summary>
    public class PersonalFinanceScenario
    {
        public object PersonalCareCategory { get; }
        public object UtilitiesCategory { get; }
        public object PersonalCareProduct { get; }
        public object UtilitiesProduct { get; }
        public object PersonalCareExpense { get; }
        public object UtilitiesExpense { get; }

        public PersonalFinanceScenario()
        {
            var personalCareCategoryId = Guid.NewGuid().ToString();
            var utilitiesCategoryId = Guid.NewGuid().ToString();

            PersonalCareCategory = E2ETestDataBuilders.Categories.PersonalCare;
            UtilitiesCategory = E2ETestDataBuilders.Categories.Utilities;

            PersonalCareProduct = E2ETestDataBuilders.CreateProductRequest("Shampoo", personalCareCategoryId, "system");
            UtilitiesProduct = E2ETestDataBuilders.CreateProductRequest("Electricity", utilitiesCategoryId, "system");

            PersonalCareExpense = E2ETestDataBuilders.CreateExpenseRequest(
                DateTime.UtcNow,
                "BOB",
                "Personal care items",
                new List<object>
                {
                    E2ETestDataBuilders.CreateExpenseItemRequest(personalCareCategoryId, "1.000", "25.0000", "Shampoo")
                });

            UtilitiesExpense = E2ETestDataBuilders.CreateExpenseRequest(
                DateTime.UtcNow,
                "BOB",
                "Monthly utilities",
                new List<object>
                {
                    E2ETestDataBuilders.CreateExpenseItemRequest(utilitiesCategoryId, "1.000", "150.0000", "Electricity bill")
                });
        }
    }
}

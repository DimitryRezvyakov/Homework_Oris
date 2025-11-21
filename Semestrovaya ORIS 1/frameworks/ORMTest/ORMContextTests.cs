using MyORMLibrary.Common;

namespace ORMTest
{
    public class UserModel()
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [TestClass]
    public class ORMContextTests
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Users;Username=postgres;Password=24092006;";
        private ORMContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new ORMContext(ConnectionString);
        }

        [TestMethod]
        public void Create_ShouldInsertNewUser()
        {
            // Arrange
            var user = new UserModel
            {
                UserName = "test_user",
                Password = "12345"
            };

            // Act
            var created = _context.Create(user);
            var all = _context.ReadByAll<UserModel>();

            // Assert
            Assert.IsNotNull(created);
            Assert.IsTrue(all.Exists(u => u.UserName == "test_user"));
        }

        [TestMethod]
        public void ReadById_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new UserModel { UserName = "read_test", Password = "abc" };
            _context.Create(user);

            var all = _context.ReadByAll<UserModel>();
            var created = all.Find(u => u.UserName == "read_test");

            // Act
            var result = _context.ReadById<UserModel>(created.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("read_test", result.UserName);
        }

        [TestMethod]
        public void ReadByAll_ShouldReturnUsersList()
        {
            // Act
            var users = _context.ReadByAll<UserModel>();

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void Update_ShouldModifyUser()
        {
            // Arrange
            var user = new UserModel { UserName = "old_name", Password = "old_pass" };
            _context.Create(user);

            var all = _context.ReadByAll<UserModel>();
            var existing = all.Find(u => u.UserName == "old_name");
            existing.UserName = "updated_name";
            existing.Password = "new_pass";

            // Act
            _context.Update(existing.Id, existing);
            var updated = _context.ReadById<UserModel>(existing.Id);

            // Assert
            Assert.IsNotNull(updated);
            Assert.AreEqual("updated_name", updated.UserName);
        }

        [TestMethod]
        public void Delete_ShouldRemoveUser()
        {
            // Arrange
            var user = new UserModel { UserName = "delete_me", Password = "pass" };
            _context.Create(user);

            var all = _context.ReadByAll<UserModel>();
            var existing = all.Find(u => u.UserName == "delete_me");

            // Act
            _context.Delete<UserModel>(existing.Id);
            var afterDelete = _context.ReadById<UserModel>(existing.Id);

            // Assert
            Assert.IsNull(afterDelete);
        }
    }
}
using MiniTemplateEngine;

namespace MiniTemplateEngineTests
{
    [TestClass]
    public sealed class HtmlTemplateRendererTests
    {
        [TestMethod]
        public void RenderFromString_When_SimpleVariable_ReplacedCorrectly()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "<h1>Привет, ${user.Name}!</h1>";
            var model = new { user = new { Name = "Дима" } };
            string expected = "<h1>Привет, Дима!</h1>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_When_IfTrue_BlockRendersIfSection()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "$if(user.IsActive)<p>User is active</p>$else<p>User is not active</p>$endif";
            var model = new { user = new { IsActive = true } };
            string expected = "<p>User is active</p>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_When_IfFalse_BlockRendersElseSection()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "$if(user.IsActive)<p>User is active</p>$else<p>User is not active</p>$endif";
            var model = new { user = new { IsActive = false } };
            string expected = "<p>User is not active</p>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_When_Foreach_RendersAllItems()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "<ul>$foreach(var item in user.Items)<li>${item.Name}</li>$endfor</ul>";
            var model = new
            {
                user = new
                {
                    Items = new[]
                    {
                        new { Name = "Apple" },
                        new { Name = "Banana" },
                        new { Name = "Orange" }
                    }
                }
            };
            string expected = "<ul><li>Apple</li><li>Banana</li><li>Orange</li></ul>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_When_NestedForeachAndIf_RendersCorrectly()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "<ul>$foreach(var item in user.Items)$if(item.IsAvailable)<li>${item.Name} - available</li>$else<li>${item.Name} - not available</li>$endif$endfor</ul>";
            var model = new
            {
                user = new
                {
                    Items = new[]
                    {
                        new { Name = "Apple", IsAvailable = true },
                        new { Name = "Banana", IsAvailable = false }
                    }
                }
            };
            string expected = "<ul><li>Apple - available</li><li>Banana - not available</li></ul>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_When_IfWithoutElse_RendersIfOnly()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();
            string template = "$if(user.IsActive)<p>User is active</p>$endif";
            var modelTrue = new { user = new { IsActive = true } };
            var modelFalse = new { user = new { IsActive = false } };
            string expectedTrue = "<p>User is active</p>";
            string expectedFalse = "";

            // act
            string resultTrue = renderer.RenderFromString(template, modelTrue);
            string resultFalse = renderer.RenderFromString(template, modelFalse);

            // assert
            Assert.AreEqual(expectedTrue, resultTrue);
            Assert.AreEqual(expectedFalse, resultFalse);
        }

        [TestMethod]
        public void RenderFromString_When_ComplexTemplate_RendersAllCorrectly()
        {
            // arrange
            HtmlTemplateRenderer renderer = new HtmlTemplateRenderer();

            string template = "<h1>${user.Name}'s Dashboard</h1>$if(user.IsActive)<p>Status: Active</p>$else<p>Status: Inactive</p>$endif<p>Age: ${user.Age}</p>$if(user.HasOrders)<h2>Orders:</h2><ul>$foreach(var order in user.Orders)<li><b>Order #${order.Id}</b> - ${order.Title}$if(order.IsPaid)<span>(Paid)</span>$else<span>(Pending)</span>$endif<ul>$foreach(var item in order.Items)<li>${item.Name} x ${item.Quantity}</li>$endfor</ul></li>$endfor</ul>$endif$if(user.HasNotifications)<h2>Notifications:</h2><ul>$foreach(var note in user.Notifications)<li>${note}</li>$endfor</ul>$endif$if(user.HasPromo)<p>Your promo code: ${user.PromoCode}</p>$endif<footer>Generated for ${user.Name}</footer>";

            var model = new
            {
                user = new
                {
                    Name = "Alex",
                    Age = 32,
                    IsActive = true,
                    HasOrders = true,
                    Orders = new[]
                    {
                        new
                        {
                            Id = 101,
                            Title = "Electronics",
                            IsPaid = true,
                            Items = new[]
                            {
                                    new { Name = "Phone", Quantity = 1 },
                                    new { Name = "Charger", Quantity = 2 }
                            }
                        },
                        new
                        {
                            Id = 102,
                            Title = "Groceries",
                            IsPaid = false,
                            Items = new[]
                            {
                                new { Name = "Apple", Quantity = 6 },
                                new { Name = "Milk", Quantity = 2 }
                            }
                        }
                    },
                    HasNotifications = true,
                    Notifications = new[] { "Welcome back!", "Your order #101 was shipped." },
                    HasPromo = true,
                    PromoCode = "DISCOUNT10"
                }
            };

            string expected = "<h1>Alex's Dashboard</h1><p>Status: Active</p><p>Age: 32</p><h2>Orders:</h2><ul><li><b>Order #101</b> - Electronics<span>(Paid)</span><ul><li>Phone x 1</li><li>Charger x 2</li></ul></li><li><b>Order #102</b> - Groceries<span>(Pending)</span><ul><li>Apple x 6</li><li>Milk x 2</li></ul></li></ul><h2>Notifications:</h2><ul><li>Welcome back!</li><li>Your order #101 was shipped.</li></ul><p>Your promo code: DISCOUNT10</p><footer>Generated for Alex</footer>";

            // act
            string result = renderer.RenderFromString(template, model);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderFromString_WhenNestedForeach_ReturnsNestedItems()
        {
            var testee = new HtmlTemplateRenderer();
            string template = "$foreach(var g in Groups)<h2>${g.Name}</h2>$foreach(var u in g.Users)<p>${u.Name}</p>$endfor$endfor";
            var model = new
            {
                Groups = new[]
                {
                new { Name = "Group1", Users = new[] { new { Name = "Alice" }, new { Name = "Bob" } } },
                new { Name = "Group2", Users = new[] { new { Name = "Charlie" } } }
            }
            };
            string expected = "<h2>Group1</h2><p>Alice</p><p>Bob</p><h2>Group2</h2><p>Charlie</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

    }
}

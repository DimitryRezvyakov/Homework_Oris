using MiniTemplateEngineTests.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTemplateEngineTests
{
    [TestClass]
    public class ForeachParserTests
    {
            [TestMethod]
            public void Parse_OnSingleForeach_ReturnsCorrect()
            {
                // Arrange
                var template = @"$foreach(var item in user.Items)
                                <li>${item.Name}</li>
                            $endfor";

                // Act
                var result = ForeachParser.Parse(template, 0);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.IterationModel);
                Assert.AreEqual("item", result.IterationModel.PropertyName);
                Assert.AreEqual("user.Items", result.IterationModel.CollectionPath);
                Assert.AreEqual("<li>${item.Name}</li>", result.Content.Trim());
                Assert.AreEqual(0, result.Start);
            }

            [TestMethod]
            public void Parse_OnDoubleForeach_ReturnsCorrect()
            {
                // Arrange
                var template = @"$foreach(var user in users)
                                    <div>
                                        $foreach(var item in user.Items)
                                            <span>${item.Name}</span>
                                        $endfor
                                    </div>
                                $endfor";

                // Act
                var result = ForeachParser.Parse(template, 0);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.IterationModel);
                Assert.AreEqual("user", result.IterationModel.PropertyName);
                Assert.AreEqual("users", result.IterationModel.CollectionPath);
                Assert.IsTrue(result.Content.Contains("$foreach(var item in user.Items)"));
                Assert.IsTrue(result.Content.Contains("<span>${item.Name}</span>"));
                Assert.AreEqual(0, result.Start);
            }

            [TestMethod]
            public void Parse_OnForeachIfForeach_ReturnsCorrect()
            {
                // Arrange
                var template = @"$foreach(var user in users)
                                    $if(user.IsActive)
                                        $foreach(var item in user.Items)
                                            <p>${item.Name}</p>
                                        $endfor
                                    $endif
                                $endfor";

                // Act
                var result = ForeachParser.Parse(template, 0);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.IterationModel);
                Assert.AreEqual("user", result.IterationModel.PropertyName);
                Assert.AreEqual("users", result.IterationModel.CollectionPath);
                Assert.IsTrue(result.Content.Contains("$if(user.IsActive)"));
                Assert.IsTrue(result.Content.Contains("$foreach(var item in user.Items)"));
                Assert.IsTrue(result.Content.Contains("<p>${item.Name}</p>"));
                Assert.AreEqual(0, result.Start);
            }

            [TestMethod]
            public void Parse_OnForeachForeachIf_ReturnsCorrect()
            {
                // Arrange
                var template = @"$foreach(var user in users)
                                    $foreach(var order in user.Orders)
                                        $if(order.IsCompleted)
                                            <div>${order.Total}</div>
                                        $endif
                                    $endfor
                                $endfor";

                // Act
                var result = ForeachParser.Parse(template, 0);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.IterationModel);
                Assert.AreEqual("user", result.IterationModel.PropertyName);
                Assert.AreEqual("users", result.IterationModel.CollectionPath);
                Assert.IsTrue(result.Content.Contains("$foreach(var order in user.Orders)"));
                Assert.IsTrue(result.Content.Contains("$if(order.IsCompleted)"));
                Assert.IsTrue(result.Content.Contains("<div>${order.Total}</div>"));
                Assert.AreEqual(0, result.Start);
            }

            [TestMethod]
            public void Parse_OnForeachWithComplexContent_ReturnsCorrect()
            {
                // Arrange
                var template = @"$foreach(var product in catalog.Products)
                                <div class='product'>
                                    <h3>${product.Name}</h3>
                                    <p>${product.Description}</p>
                                    <span>$${product.Price}</span>
                                    $if(product.InStock)
                                        <button>Add to Cart</button>
                                    $else
                                        <p>Out of Stock</p>
                                    $endif
                                </div>
                            $endfor";

                // Act
                var result = ForeachParser.Parse(template, 0);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.IterationModel);
                Assert.AreEqual("product", result.IterationModel.PropertyName);
                Assert.AreEqual("catalog.Products", result.IterationModel.CollectionPath);
                Assert.IsTrue(result.Content.Contains("$if(product.InStock)"));
                Assert.IsTrue(result.Content.Contains("<button>Add to Cart</button>"));
                Assert.IsTrue(result.Content.Contains("<p>Out of Stock</p>"));
                Assert.AreEqual(0, result.Start);
            }
        }
    }

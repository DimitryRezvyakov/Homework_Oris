using MiniTemplateEngineTests.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTemplateEngineTests
{
    [TestClass]
    public class IfParserTests
    {
        [TestMethod]
        public void Parse_OnSingleIfWithoutElse_ReturnCorrect()
        {
            // Arrange
            var template = "$if(user.IsActive)<p>User is active</p>$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.AreEqual("<p>User is active</p>", result.IfContent);
            Assert.AreEqual("", result.ElseContent);
            Assert.AreEqual(template.Length, result.Length);
        }

        [TestMethod]
        public void Parse_OnDoubleIfWithoutElse_ReturnCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <p>User is active</p>
    $if(user.IsPremium)
        <p>Premium user</p>
    $endif
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$if(user.IsPremium)"));
            Assert.IsTrue(result.IfContent.Contains("<p>Premium user</p>"));
            Assert.AreEqual("", result.ElseContent);
        }

        [TestMethod]
        public void Parse_OnDoubleIfWhereInternalIfHasElse_ReturnsCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <p>User is active</p>
    $if(user.IsPremium)
        <p>Premium user</p>
    $else
        <p>Regular user</p>
    $endif
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$if(user.IsPremium)"));
            Assert.IsTrue(result.IfContent.Contains("$else"));
            Assert.IsTrue(result.IfContent.Contains("<p>Regular user</p>"));
            Assert.AreEqual("", result.ElseContent);
        }

        [TestMethod]
        public void Parse_OnDoubleIfWhereExternalIfHasElse_ReturnsCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <p>User is active</p>
    $if(user.IsPremium)
        <p>Premium user</p>
    $endif
$else
    <p>User is not active</p>
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$if(user.IsPremium)"));
            Assert.IsTrue(result.IfContent.Contains("<p>Premium user</p>"));
            Assert.AreEqual("<p>User is not active</p>", result.ElseContent.Trim());
        }

        [TestMethod]
        public void Parse_OnDoubleIfWhereBothHasElse_ReturnsCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <p>User is active</p>
    $if(user.IsPremium)
        <p>Premium user</p>
    $else
        <p>Regular user</p>
    $endif
$else
    <p>User is not active</p>
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$if(user.IsPremium)"));
            Assert.IsTrue(result.IfContent.Contains("$else"));
            Assert.IsTrue(result.IfContent.Contains("<p>Regular user</p>"));
            Assert.AreEqual("<p>User is not active</p>", result.ElseContent.Trim());
        }

        [TestMethod]
        public void ParseOnIfForeachIfWhereInternalIfHasElse()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <ul>
    $foreach(var item in user.Items)
        $if(item.IsAvailable)
            <li>${item.Name} - Available</li>
        $else
            <li>${item.Name} - Not Available</li>
        $endif
    $endfor
    </ul>
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$foreach(var item in user.Items)"));
            Assert.IsTrue(result.IfContent.Contains("$if(item.IsAvailable)"));
            Assert.IsTrue(result.IfContent.Contains("$else"));
            Assert.IsTrue(result.IfContent.Contains("<li>${item.Name} - Not Available</li>"));
            Assert.AreEqual("", result.ElseContent);
        }

        [TestMethod]
        public void ParseOnIfForeachIfWhereExternalIfHasElse()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    <ul>
    $foreach(var item in user.Items)
        <li>${item.Name}</li>
    $endfor
    </ul>
$else
    <p>No active user</p>
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("$foreach(var item in user.Items)"));
            Assert.IsTrue(result.IfContent.Contains("<li>${item.Name}</li>"));
            Assert.AreEqual("<p>No active user</p>", result.ElseContent.Trim());
        }

        [TestMethod]
        public void Parse_OnIfWithElse_ReturnCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
<p>User is active</p>
$else
<p>User is not active</p>
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.AreEqual("<p>User is active</p>", result.IfContent.Trim());
            Assert.AreEqual("<p>User is not active</p>", result.ElseContent.Trim());
        }

        [TestMethod]
        public void Parse_OnNestedIfWithDifferentConditions_ReturnCorrect()
        {
            // Arrange
            var template = @"$if(user.IsActive)
    $if(user.Role == ""Admin"")
        <p>Admin user</p>
    $endif
$endif";

            // Act
            var result = IfParser.Parse(template, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user.IsActive", result.Condition.PropertyPath);
            Assert.IsTrue(result.IfContent.Contains("user.Role == \"Admin\""));
            Assert.IsTrue(result.IfContent.Contains("<p>Admin user</p>"));
        }
    }
}


using System.Collections.Generic;
using Promitor.Agents.ResourceDiscovery.Graph.Query;
using Xunit;

namespace Promitor.Tests.Unit.Builders.Queries
{
    public class GraphQueryBuilderTests
    {
        private const string ResourceType = "resource type";

        [Fact]
        public void ForResourceType_SingleResourceType_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'";

            // Act
            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void ForResourceType_TwoResourceTypes_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string ResourceType1 = "resource type 1";
            const string ResourceType2 = "resource type 2";
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type 1'\r\n" +
                " or type =~ 'resource type 2'";

            // Act
            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(new[] { ResourceType1, ResourceType2 });

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Theory]
        [InlineData(Operator.Contains, "contains")]
        [InlineData(Operator.DoesNotContain, "!contains")]
        [InlineData(Operator.DoesNotEquals, "!=")]
        [InlineData(Operator.Equals, "==")]
        public void Where_AppendCorrectQuery(Operator @operator, string queryOperatopr)
        {
            // Arrange
            string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| where field " + queryOperatopr + " 'value'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Where("field", @operator, "value");

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithSubscriptionsWithIds_AppendCorrectQuery()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| where subscriptionId =~ 'subscription Id 1' or subscriptionId =~ 'subscription Id 2' or subscriptionId =~ 'subscription Id 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> subscriptionIds = new List<string>
            {
                "subscription Id 1",
                "subscription Id 2",
                "subscription Id 3"
            };

            // Act
            graphQueryBuilder.WithSubscriptionsWithIds(subscriptionIds);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithResourceGroupsWithName_AppendCorrectQuery()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| where resourceGroup =~ 'resourceGroup 1' or resourceGroup =~ 'resourceGroup 2' or resourceGroup =~ 'resourceGroup 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> resourceGroups = new List<string>
            {
                "resourceGroup 1",
                "resourceGroup 2",
                "resourceGroup 3"
            };

            // Act
            graphQueryBuilder.WithResourceGroupsWithName(resourceGroups);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithinRegions_AppendCorrectQuery()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| where location =~ 'region 1' or location =~ 'region 2' or location =~ 'region 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            List<string> regions = new List<string>
            {
                "region 1",
                "region 2",
                "region 3"
            };

            // Act
            graphQueryBuilder.WithinRegions(regions);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void WithinTags_AppendCorrectQuery()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| where tags['tag key 1'] == 'tag value 1' or tags['tag key 2'] == 'tag value 2' or tags['tag key 3'] == 'tag value 3'";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            Dictionary<string, string> tags = new Dictionary<string, string>
            {
                ["tag key 1"] = "tag value 1",
                ["tag key 2"] = "tag value 2",
                ["tag key 3"] = "tag value 3",
            };

            // Act
            graphQueryBuilder.WithTags(tags);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void Project_SingleField_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| project field";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Project("field");

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Fact]
        public void Project_TwoFields_ReturnsValidGraphQueryBuilder()
        {
            // Arrange
            const string Field1 = "field 1";
            const string Field2 = "field 2";

            const string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| project field 1, field 2";

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.Project(new[] { Field1, Field2 });

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(17)]
        [InlineData(57)]
        public void Limit2_AppendCorrectQuery(int limit)
        {
            // Arrange
            string ExpectedQuery =
                "Resources\r\n" +
                "| where type =~ 'resource type'\r\n" +
                "| limit " + limit;

            GraphQueryBuilder graphQueryBuilder = GraphQueryBuilder.ForResourceType(ResourceType);

            // Act
            graphQueryBuilder.LimitTo(limit);

            // Assert
            Assert.Equal(ExpectedQuery, graphQueryBuilder.Build());
        }
    }
}

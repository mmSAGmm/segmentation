using Moq;
using Moq.AutoMock;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Implementation;
using Segmentation.DomainModels;
using Shouldly;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class EvaluationServiceTests
    {
        public AutoMocker mocker = new AutoMocker();

        public EvaluationService Subject => mocker.Get<EvaluationService>();

        public EvaluationServiceTests()
        {
            mocker.Use<IExpressionService>(new ExpressionService());
        }

        public void WithExpression(string expression)
        {
            var mock = mocker.GetMock<ISegmentAdminService>();
            mock.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new Segment() { Expression = expression });
        }

        private void WithProperties(Dictionary<string, object> properties)
        {
            var mock = mocker.GetMock<IPropertiesService>();
            mock.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(properties);
        }

        [Fact]
        public async Task BaseEvaluation()
        {
            WithProperties(new Dictionary<string, object> { });
            WithExpression("1==1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty);
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task NumberEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = 1 });
            WithExpression(@"x.name == 1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty);
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task StringEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = "1" });
            WithExpression(@"x.name == ""1""");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty);
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AndEvaluation()
        {
            WithProperties(new Dictionary<string, object>
            {
                ["name"] = "1",
                ["name1"] = 2,
                ["name2"] = "1",
            });
            WithExpression(@"x.name == ""1"" && x.name1 == 2");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty);
            result.ShouldBeTrue();
        }
    }
}
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Implementation;
using Segmentation.Domain.Options;
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
            mocker.Use<IExpressionCompilationService>(new ExpressionCompilationService());
            mocker.Use<IOptions<EvaluationOption>>(Options.Create<EvaluationOption>(new EvaluationOption()));
        }

        public void WithExpression(string expression)
        {
            var mock = mocker.GetMock<ISegmentAdminService>();
            mock.Setup(x => x.Get(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(new Segment() { Expression = expression });
        }

        private void WithProperties(Dictionary<string, object> properties)
        {
            var mock = mocker.GetMock<IPropertiesService>();
            mock.Setup(x => x.Get(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(properties);
        }

        [Fact]
        public async Task BaseEvaluation()
        {
            WithProperties(new Dictionary<string, object> { });
            WithExpression("1==1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }

        [Fact]
        public async Task NumberEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = 1 });
            WithExpression(@"x.name == 1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }


        [Fact]
        public async Task NumberWithPlusEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = 1 });
            WithExpression(@"x.name+1 == 2");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }
        [Fact]
        public async Task NumberDivideLeftEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = 11 });
            WithExpression(@"x.name%10 == 1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }

        [Fact]
        public async Task InvertBoolEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["b"] = false });
            WithExpression(@"x.b == false");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }

        [Fact]
        public async Task BoolEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["b"] = true });
            WithExpression(@"x.b == true");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }

        [Fact]
        public async Task TypeMissMatchEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = "1" });
            WithExpression(@"x.name == 1");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(false);
        }

        [Fact]
        public async Task StringEvaluation()
        {
            WithProperties(new Dictionary<string, object> { ["name"] = "1" });
            WithExpression(@"x.name == ""1""");
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
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
            var result = await Subject.Evaluate(Guid.Empty, string.Empty, CancellationToken.None);
            result.ShouldBe(true);
        }
    }
}
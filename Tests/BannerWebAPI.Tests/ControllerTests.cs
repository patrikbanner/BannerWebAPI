using BannerWebAPI.Controllers;
using BannerWebAPI.Mappers;
using BannerWebAPI.Models;
using BannerWebAPI.Repositories;
using BannerWebAPI.Validation;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BannerWebAPI.Tests
{
    public class ControllerTests
    {
        private readonly BannerController _controller;
        private readonly IBannerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidate _validateHtml;

        public ControllerTests()
        {
            _repository = A.Fake<IBannerRepository>();
            _mapper = A.Fake<IMapper>();
            _validateHtml = A.Fake<IValidate>();

            _controller = new BannerController(_repository, _mapper, _validateHtml);
        }

        [Fact]
        public void Should_return_banner_with_correct_id()
        {
            var banner = new Banner { Id = 1 };
            A.CallTo(() => _repository.TryGet(A<int>.Ignored, out banner))
                .Invokes((call) =>
                {
                    var a = call.Arguments.ArgumentNames;
                })
                .Returns(true)
                .AssignsOutAndRefParameters(banner);

            var result = _controller.GetBanner(1) as ObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<Banner>();

            ((Banner)result.Value).Id.Should().Be(1);
        }

        [Fact]
        public void Should_create_banner_with_correct_id()
        {
            var createBanner = new PostBanner { Id = 1, Html = "html1" };
            var mappedPostBanner = new Banner { Id = createBanner.Id, Html = createBanner.Html };

            Banner savedBanner = null;
            PostBanner mappedBanner = null;
            A.CallTo(() => _repository.TrySave(A<Banner>.Ignored))
                .Invokes((Banner banner) => savedBanner = banner)
                .Returns(true);

            A.CallTo(() => _mapper.Map(A<PostBanner>.Ignored))
                .Invokes((PostBanner banner) => mappedBanner = banner)
                .Returns(mappedPostBanner);

            var result = _controller.PostBanner(createBanner) as StatusCodeResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            mappedBanner.Id.Should().Be(1);
            mappedBanner.Html.Should().Be("html1");

            savedBanner.Id.Should().Be(1);
            savedBanner.Html.Should().Be("html1");
        }

        [Fact]
        public void Should_update_banner_with_correct_id()
        {
            var updateBanner = new PutBanner { Html = "html2" };
            var mappedPutBanner = new Banner { Id = 1, Html = updateBanner.Html };

            var storedBanner = new Banner { Id = 1, Html = "html1" };
            A.CallTo(() => _repository.TryGet(A<int>.Ignored, out storedBanner))
                .Returns(true)
                .AssignsOutAndRefParameters(storedBanner);

            Banner savedBanner = null;
            A.CallTo(() => _repository.TrySave(A<Banner>.Ignored))
                .Invokes((Banner banner) => savedBanner = banner)
                .Returns(true);

            Banner oldBanner = null;
            PutBanner newBanner = null;
            A.CallTo(() => _mapper.Map(A<Banner>.Ignored, A<PutBanner>.Ignored))
                .Invokes((Banner banner, PutBanner putBanner) =>
                {
                    oldBanner = banner;
                    newBanner = putBanner;
                })
                .Returns(mappedPutBanner);

            var result = _controller.PutBanner(1, updateBanner) as StatusCodeResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            oldBanner.Id.Should().Be(1);
            oldBanner.Html.Should().Be("html1");

            newBanner.Html.Should().Be("html2");

            savedBanner.Id.Should().Be(1);
            savedBanner.Html.Should().Be("html2");
        }

        [Fact]
        public void Should_remove_banner_with_correct_id()
        {
            A.CallTo(() => _repository.Remove(A<int>.Ignored))
                .Returns(true);

            var result = _controller.DeleteBanner(1) as StatusCodeResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
using BannerWebAPI.Mappers;
using BannerWebAPI.Models;
using BannerWebAPI.Repositories;
using BannerWebAPI.Validation;
using Microsoft.AspNetCore.Mvc;

namespace BannerWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidate _validateHtml;

        public BannerController(IBannerRepository repository, IMapper mapper, IValidate validate)
        {
            _repository = repository;
            _mapper = mapper;
            _validateHtml = validate;
        }

        [HttpGet]
        [Route("banners", Name = "GetBanners")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Banner>), (int)HttpStatusCode.OK)]
        public IActionResult GetBanners()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.TryGetAll(out var banners))
            {
                return NoContent();
            }

            return Ok(banners);
        }

        [HttpGet]
        [Route("banners/{bannerId}", Name = "GetBanner")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Banner), (int)HttpStatusCode.OK)]
        public IActionResult GetBanner([FromRoute] int bannerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.TryGet(bannerId, out var banner))
            {
                return NoContent();
            }

            return Ok(banner);
        }

        [HttpGet]
        [Route("banners/{bannerId}/html", Name = "GetBannerHTML")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [Produces("text/html")]
        public IActionResult GetBannerHTML([FromRoute] int bannerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.TryGet(bannerId, out var banner))
            {
                return NoContent();
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = banner.Html
            };
        }

        [HttpPost]
        [Route("banners", Name = "PostBanner")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public IActionResult PostBanner([FromBody] PostBanner postBanner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parseErrors = _validateHtml.Validate(postBanner.Html);

            if (parseErrors.Any())
            {
                return BadRequest(new ErrorResponse(parseErrors));
            }

            if (!_repository.TrySave(_mapper.Map(postBanner)))
            {
                return BadRequest($"Failed to save banner");
            }

            return Ok();
        }

        [HttpPut]
        [Route("banners/{bannerId}", Name = "PutBanner")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public IActionResult PutBanner([FromRoute] int bannerId, [FromBody] PutBanner putBanner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parseErrors = _validateHtml.Validate(putBanner.Html);

            if (parseErrors.Any())
            {
                return BadRequest(new ErrorResponse(parseErrors));
            }

            if (!_repository.TryGet(bannerId, out var banner))
            {
                return BadRequest($"Banner with {bannerId} not found");
            }

            if (!_repository.TrySave(_mapper.Map(banner, putBanner)))
            {
                return BadRequest($"Failed to save banner {bannerId}");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("banners/{bannerId}", Name = "DeleteBanner")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Banner), (int)HttpStatusCode.OK)]
        public IActionResult DeleteBanner([FromRoute] int bannerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.Remove(bannerId))
            {
                return BadRequest($"Banner with {bannerId} not found");
            }

            return Ok();
        }
    }
}

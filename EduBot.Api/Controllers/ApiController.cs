using System.Diagnostics;
using EduBot.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduBot.Api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase {
        private readonly IMediator _mediator;

        protected ApiController(IMediator mediator) =>
            _mediator = mediator;

        private string TraceId => Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        private string Path => HttpContext.Request.Path.ToString();

        protected async Task<TResult> QueryAsync<TResult>(
            IRequest<TResult> query,
            CancellationToken cancellationToken = default
        ) {
            try {
                return await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e) {
                throw;
            }
        }

        protected async Task<TResult> CommandAsync<TResult>(
            IRequest<TResult> command,
            CancellationToken cancellationToken = default
        ) {
            try {
                return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e) {
                throw;
            }
        }

        protected new IActionResult Ok() {
            return base.Ok();
        }

        protected IActionResult Ok<T>(T result) {
            return base.Ok(result);
        }

        protected IActionResult Created<T>(T result) {
            string location = Url.Action(HttpContext.Request.Path) ?? "";
            return Created(location, result);
        }

        protected IActionResult Created() {
            string location = Url.Action(HttpContext.Request.Path) ?? "";
            return Created(location);
        }

        protected IActionResult Problem(List<Error> errors) {
            if (errors.Count is 0) {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation)) {
                return ValidationProblem(errors);
            }

            HttpContext.Items["errors"] = errors;

            return Problem(errors[0]);
        }

        private IActionResult Problem(Error error) {
            int statusCode = error.Type switch {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var result = ErrorModel.Create(statusCode, TraceId, error.Description);

            return new ObjectResult(result) { StatusCode = statusCode };
        }

        private IActionResult ValidationProblem(List<Error> errors) {
            const int statusCode = StatusCodes.Status400BadRequest;
            var result = ErrorModel.Create(statusCode, TraceId, errors);

            return new ObjectResult(result) { StatusCode = statusCode };
        }
    }
}
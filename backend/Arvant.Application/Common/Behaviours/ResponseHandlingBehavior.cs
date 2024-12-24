using Arvant.Application.Common.Models;

namespace Arvant.Application.Common.Behaviours;

public class ResponseHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest: notnull
    where TResponse: IResult<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken) {
        try {
            return await next();
        } catch (Exception ex) {
            return TResponse.Failure([ex.Message]);
        }
    }
}

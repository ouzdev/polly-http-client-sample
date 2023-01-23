using ApiClient.Services;
using MediatR;

namespace ApiClient.Application.Queries;

public class GetSayHelloQueryHandler : IRequestHandler<GetSayHelloQuery, string>

{
    private readonly ISayHelloHttpService _sayHelloHttpService;

    public GetSayHelloQueryHandler(ISayHelloHttpService sayHelloHttpService)
    {
        _sayHelloHttpService = sayHelloHttpService;
    }

    public async Task<string> Handle(GetSayHelloQuery request, CancellationToken cancellationToken)
    {
        return  await _sayHelloHttpService.GetHelloAsync(new CancellationToken());
       
    }
}
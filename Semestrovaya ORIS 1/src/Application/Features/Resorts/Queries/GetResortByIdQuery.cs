using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Resorts.Queries
{
    public class GetResortByIdQuery : IRequest<Result<Resort>>
    {
        public int Id { get; set; }

        public GetResortByIdQuery() { }

        public GetResortByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetResortByIdQueryHandler : IRequestHandler<GetResortByIdQuery, Result<Resort>>
    {
        private readonly IGenericRepository _repository;
        public GetResortByIdQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Resort>> Handle(GetResortByIdQuery request, CancellationToken cts)
        {
            try
            {
                var resort = await _repository.GetById<Resort>(request.Id);

                if (resort != null)
                    return await Result<Resort>.SuccessAsync(resort);

                return await Result<Resort>.FailureAsync($"Can`t get resort with id {request.Id}");
            }
            catch (Exception ex)
            {
                return await Result<Resort>.FailureAsync(ex);
            }
        }
    }
}

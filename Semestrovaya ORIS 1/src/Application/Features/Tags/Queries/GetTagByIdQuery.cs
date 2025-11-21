using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tags.Queries
{
    public class GetTagByIdQuery : IRequest<Result<HotelTag>>
    {
        public int Id { get; set; }
        public GetTagByIdQuery() { }
        public GetTagByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<HotelTag>>
    {
        private readonly IGenericRepository _repository;

        public GetTagByIdQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<HotelTag>> Handle(GetTagByIdQuery request, CancellationToken cts)
        {
            try
            {
                var result = await _repository.GetById<HotelTag>(request.Id);

                if (result != null)
                    return await Result<HotelTag>.SuccessAsync(result);

                return await Result<HotelTag>.FailureAsync($"Can`t find Tag with Id {request.Id}");
            }

            catch (Exception ex)
            {
                return await Result<HotelTag>.FailureAsync(ex);
            }
        }
    }
}

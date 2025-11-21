using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotels.Queries
{
    public class GetHotelByIdQuery : IRequest<Result<Hotel>>
    {
        public int Id { get; set; }

        public GetHotelByIdQuery() { }

        public GetHotelByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, Result<Hotel>>
    {
        private readonly IGenericRepository _repository;

        public GetHotelByIdQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Hotel>> Handle(GetHotelByIdQuery request, CancellationToken cts)
        {
            try
            {
                var data = await _repository.GetById<Hotel>(request.Id);

                if (data != null)
                    return await Result<Hotel>.SuccessAsync(data);

                return await Result<Hotel>.FailureAsync($"Hotel with id {request.Id} was not found");
            }
            catch (Exception ex)
            {
                return await Result<Hotel>.FailureAsync(ex);
            }
        }
    }
}

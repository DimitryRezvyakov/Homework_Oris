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
    public class GetAllTagsQuery : IRequest<Result<List<HotelTag>>>
    {
    }

    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, Result<List<HotelTag>>>
    {
        private readonly IGenericRepository _repository;
        public GetAllTagsQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<HotelTag>>> Handle(GetAllTagsQuery request, CancellationToken cts)
        {
            try
            {
                var data = await _repository.GetAll<HotelTag>();

                if (data != null)
                    return await Result<List<HotelTag>>.SuccessAsync(data);

                return await Result<List<HotelTag>>.FailureAsync("Can`t find Hotel tags");
            }

            catch (Exception ex)
            {
                return await Result<List<HotelTag>>.FailureAsync(ex);
            }
        }
    }
}

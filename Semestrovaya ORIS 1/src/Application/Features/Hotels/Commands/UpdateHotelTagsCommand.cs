using Application.Entities;
using Application.Services.Repositories;
using Common;
using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotels.Commands
{
    public class UpdateHotelTagsCommand : IRequest<Result<bool>>
    {
        public int HotelId { get; set; }
        public int[] TagsId { get; set; }
        public UpdateHotelTagsCommand() { }
        public UpdateHotelTagsCommand(int hotelId, int[] tagsId)
        {
            HotelId = hotelId;
            TagsId = tagsId;
        }
    }

    internal class UpdateHotelTagsCommandHandler : IRequestHandler<UpdateHotelTagsCommand, Result<bool>>
    {
        private readonly IGenericRepository _repository;

        public UpdateHotelTagsCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(UpdateHotelTagsCommand request, CancellationToken cts)
        {
            try
            {
                foreach (var id in request.TagsId)
                {
                    await _repository.Create<HotelTagTable>(new HotelTagTable()
                    {
                        HotelId = request.HotelId,
                        TagId = id
                    });
                }

                return await Result<bool>.SuccessAsync(true);
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailureAsync(ex);
            }
        }
    }
}

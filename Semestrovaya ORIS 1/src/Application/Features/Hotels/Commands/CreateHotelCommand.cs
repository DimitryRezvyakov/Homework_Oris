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
    public class CreateHotelCommand : IRequest<Result<bool>>
    {
        public Hotel Hotel { get; set; }

        public CreateHotelCommand() { }
        public CreateHotelCommand(Hotel hotel)
        {
            Hotel = hotel;
        }
    }

    internal class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<bool>>
    {
        private readonly IGenericRepository _repository;

        public CreateHotelCommandHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(CreateHotelCommand request, CancellationToken cts)
        {
            try
            {
                await _repository.Create<Hotel>(request.Hotel);

                return await Result<bool>.SuccessAsync();
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailureAsync(ex);
            }
        }
    }
}

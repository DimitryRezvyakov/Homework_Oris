using Application.DTOs;
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
    public class GetAllHotelsByFilterQuery : IRequest<Result<List<HotelMediumDTO>>>
    {
        public DestinationDto? Destination { get; set; }
        public DateRangeDto? DateRange { get; set; }
        public BudgetDto? Budget { get; set; }
        public GuestsDto? Guests { get; set; }
        public string? Meal { get; set; }
        public decimal? MinRating { get; set; }
        public int? MinStars { get; set; }
        public List<int>? Amenities { get; set; }
    }

    public class GetAllHotelsByFilterQueryHandler : IRequestHandler<GetAllHotelsByFilterQuery, Result<List<HotelMediumDTO>>>
    {
        private readonly IGenericRepository _repository;

        public GetAllHotelsByFilterQueryHandler(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<HotelMediumDTO>>> Handle(GetAllHotelsByFilterQuery request, CancellationToken cts)
        {
            try
            {
                var sqlParts = new List<string>();
                var parameters = new Dictionary<string, object>();

                // 1. Базовый SELECT + FROM
                var fromClause = new StringBuilder(@"
            SELECT 
                h.""Id"",
                h.""ResortId"",
                h.""Name"",
                h.""Price"",
                h.""Stars"",
                h.""Raiting"",
                h.""Description"",
                h.""Images""->>0 AS ""Image"",
                CONCAT(c.""Name"", ', ', r.""Name"") AS ""Location""
            FROM ""Hotel"" h
            INNER JOIN ""Resort"" r ON h.""ResortId"" = r.""Id""
            INNER JOIN ""Country"" c ON r.""CountryId"" = c.""Id""
        ");

                bool hasAmenities = request.Amenities?.Any() == true;
                if (hasAmenities)
                {
                        fromClause.Append(@"
                    INNER JOIN ""HotelTagTable"" htt ON h.""Id"" = htt.""HotelId""
                    INNER JOIN ""HotelTag"" ht ON htt.""TagId"" = ht.""Id""
                ");
                }

                sqlParts.Add(fromClause.ToString());

                var whereConditions = new List<string>();

                if (request.Destination != null)
                {
                    switch (request.Destination.Type)
                    {
                        case "country":
                            whereConditions.Add(@"c.""Id"" = @destinationId");
                            parameters["destinationId"] = request.Destination.Id;
                            break;
                        case "resort":
                            whereConditions.Add(@"h.""ResortId"" = @destinationId");
                            parameters["destinationId"] = request.Destination.Id;
                            break;
                        case "hotel":
                            whereConditions.Add(@"h.""Id"" = @destinationId");
                            parameters["destinationId"] = request.Destination.Id;
                            break;
                    }
                }

                if (request.Budget != null && request.Budget.Amount > 0)
                {
                    whereConditions.Add(@"h.""Price"" <= @budgetAmount");
                    parameters["budgetAmount"] = request.Budget.Amount;
                }

                if (!string.IsNullOrEmpty(request.Meal))
                {
                    whereConditions.Add(@"h.""Nutrition"" = @meal");
                    parameters["meal"] = request.Meal;
                }

                if (request.MinRating.HasValue && request.MinRating.Value > 0)
                {
                    whereConditions.Add(@"h.""Raiting"" >= @minRating");
                    parameters["minRating"] = request.MinRating.Value;
                }

                if (request.MinStars.HasValue && request.MinStars.Value > 0)
                {
                    whereConditions.Add(@"h.""Stars"" >= @minStars");
                    parameters["minStars"] = request.MinStars.Value;
                }

                if (hasAmenities)
                {
                    var uniqueTagIds = request.Amenities.Distinct().ToList();
                    var tagParams = new List<string>();
                    for (int i = 0; i < uniqueTagIds.Count; i++)
                    {
                        var paramName = $"tag{i}";
                        tagParams.Add($"@{paramName}");
                        parameters[paramName] = uniqueTagIds[i];
                    }
                    whereConditions.Add($"ht.\"Id\" IN ({string.Join(",", tagParams)})");
                }

                if (whereConditions.Any())
                {
                    sqlParts.Add("WHERE " + string.Join(" AND ", whereConditions));
                }

                if (hasAmenities)
                {
                    sqlParts.Add(@"
                GROUP BY 
                    h.""Id"", h.""ResortId"", h.""Name"", h.""Price"", 
                    h.""Stars"", h.""Raiting"", h.""Description"", 
                    h.""Images"", c.""Name"", r.""Name""
                HAVING COUNT(DISTINCT ht.""Id"") = @tagCount
            ");
                    parameters["tagCount"] = request.Amenities.Distinct().Count();
                }

                var sql = string.Join(" ", sqlParts);

                var data = await _repository.UseSqlCommandQueryCollection<HotelMediumDTO>(sql, parameters);

                if (data != null)
                    return await Result<List<HotelMediumDTO>>.SuccessAsync(data);

                return await Result<List<HotelMediumDTO>>.FailureAsync($"Can't find any hotels by filter.");
            }
            catch (Exception ex)
            {
                return await Result<List<HotelMediumDTO>>.FailureAsync(ex);
            }
        }
    }
}

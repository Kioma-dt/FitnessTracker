using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Shared.DTO.Responses
{
    public record PagedResponseDTO<T>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public List<T> Items { get; init; }

        public PagedResponseDTO(List<T> items, int page, int pageSize, int totalRecords)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        }
    }
}

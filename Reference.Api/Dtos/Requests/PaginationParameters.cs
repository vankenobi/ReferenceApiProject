using System;
namespace Reference.Api.Dtos.Requests
{
	public class PaginationParameters
	{
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}


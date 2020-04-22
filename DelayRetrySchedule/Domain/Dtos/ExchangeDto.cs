using System.Diagnostics.CodeAnalysis;

namespace Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ExchangeDto
    {
        public string ExhangeName { get; set; }
        public string ExchangeType { get; set; }
    }
}
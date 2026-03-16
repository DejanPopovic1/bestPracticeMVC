using System.Text.Json;

namespace WebApplication1.Data
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AnnualIncome { get; set; }
        public JsonDocument AdditionalInformation { get; set; }
    }
}

namespace Bakalauras.Core.DTOs
{
    public class MechanicDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
    }

    public class CreateMechanicDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
    }

    public class UpdateMechanicDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
    }
} 
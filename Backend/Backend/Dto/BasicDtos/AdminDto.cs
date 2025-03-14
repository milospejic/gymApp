namespace Backend.Dto.BasicDtos
{
    /// <summary>
    /// Data Transfer Object for Admin entity.
    /// </summary>
    public class AdminDto
    {
        /// <summary>
        /// Unique identifier for the admin.
        /// </summary>
        public Guid AdminId { get; set; }

        /// <summary>
        /// First name of the admin.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Surname of the admin.
        /// </summary>
        public string AdminSurname { get; set; }

        /// <summary>
        /// Email address of the admin.
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Contact phone number of the admin.
        /// </summary>
        public string AdminPhone { get; set; }
    }
}
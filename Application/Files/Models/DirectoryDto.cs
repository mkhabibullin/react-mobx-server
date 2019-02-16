using System;

namespace Application.Files.Models
{
    public class DirectoryDto
    {
        public string Name { get; }

        public DateTime CreatedAt { get; }

        public DirectoryDto(string name, DateTime createdAt)
        {
            Name = name;
            CreatedAt = createdAt;
        }
    }
}

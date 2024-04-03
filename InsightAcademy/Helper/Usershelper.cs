using InsightAcademy.Dtos;
using InsightAcademy.Entities;

namespace InsightAcademy.Helper
{
    public class Usershelper
    {
        public User DtotoEntity(UserDto userDto)
        {
            return new User()
            {
                Username = userDto.Username,
                FullName= userDto.FullName,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
                ProfileImage= ConvertIFormFileToByteArray(userDto.ProfileImage),
                Phone= userDto.Phone,
                Website = userDto.Website,
                StreetAddress= userDto.StreetAddress,
                City= userDto.City,
                Country= userDto.Country,


            };



        }
        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            if (file == null)
            {
                return new byte[0]; // Return an empty byte array if file is null
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }



    }
}

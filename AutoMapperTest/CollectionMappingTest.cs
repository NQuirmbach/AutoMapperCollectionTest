using AutoMapper;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutoMapperTest
{
    public class CollectionMappingTest
    {
        private readonly IMapper _mapper;
        private readonly IList<User> _users;
        private readonly IList<UserDto> _dtos;

        public CollectionMappingTest()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            config.AssertConfigurationIsValid();

            _mapper = new Mapper(config);

            _users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now.AddDays(-1),
                    Username = "Testuser"
                }
            };
            _dtos = new List<UserDto>
            {
                new UserDto
                {
                    Id = _users[0].Id,
                    Username = _users[0].Username
                },
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = "NewUser",
                }
            };
        }

        [Fact]
        public void MapCollection()
        {
            _mapper.Map(_dtos, _users);

            Assert.NotEqual(DateTime.MinValue, _users[0].CreationDate);
        }

        [Fact]
        public void MapSingle()
        {
            _mapper.Map(_dtos[0], _users[0]);

            Assert.NotEqual(DateTime.MinValue, _users[0].CreationDate);
        }
    }

    class User
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }
    }
    class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }

    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(m => m.CreationDate, o => o.Ignore());
        }
    }
}

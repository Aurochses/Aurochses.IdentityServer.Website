using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aurochses.Identity.EntityFrameworkCore;
using Aurochses.IdentityServer.WebSite.App.IdentityServer;
using Aurochses.Testing;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.App.IdentityServer
{
    public class IdentityProfileServiceTests
    {
        private const string SubjectId = "testSubjectId";

        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUserClaimsPrincipalFactory<ApplicationUser>> _mockUserClaimsPrincipalFactory;

        private readonly Mock<ProfileDataRequestContext> _mockProfileDataRequestContext;
        private readonly Mock<IsActiveContext> _mockIsActiveContext;

        public IdentityProfileServiceTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict).Object, null, null, null, null, null, null, null, null);
            _mockUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>(MockBehavior.Strict);

            _mockProfileDataRequestContext = new Mock<ProfileDataRequestContext>(MockBehavior.Strict);
            _mockIsActiveContext = new Mock<IsActiveContext>(MockBehavior.Strict, new ClaimsPrincipal(), new Client(), "testCaller");
        }

        private static ApplicationUser GetApplicationUser(string methodName)
        {
            return new ApplicationUser
            {
                Id = new Guid("00000000-0000-0000-0000-000000000000"),
                UserName = "john.black",
                FirstName = "John",
                LastName = "Black",
                Email = typeof(IdentityProfileServiceTests).GenerateEmail(methodName),
                EmailConfirmed = true,
                PhoneNumber = "+375297841506",
                PhoneNumberConfirmed = true
            };
        }

        public static ClaimsPrincipal GetSubject()
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Subject, SubjectId)
                    }
                )
            );
        }

        [Fact]
        public async Task GetProfileDataAsync_ThrowsArgumentNullException_WhenSubjectIsNull()
        {
            // Arrange
            _mockProfileDataRequestContext.Object.Subject = null;

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => identityProfileService.GetProfileDataAsync(_mockProfileDataRequestContext.Object));
            Assert.Equal(nameof(_mockProfileDataRequestContext.Object.Subject), exception.ParamName);
        }

        [Fact]
        public async Task GetProfileDataAsync_ThrowsException_WhenUserIsNull()
        {
            // Arrange
            _mockProfileDataRequestContext.Object.Subject = GetSubject();

            _mockUserManager
                .Setup(x => x.FindByIdAsync(SubjectId))
                .ReturnsAsync(() => null);

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => identityProfileService.GetProfileDataAsync(_mockProfileDataRequestContext.Object));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task GetProfileDataAsync_Success()
        {
            // Arrange
            _mockProfileDataRequestContext.Object.Subject = GetSubject();

            var user = GetApplicationUser(nameof(GetProfileDataAsync_Success));

            _mockUserManager
                .Setup(x => x.FindByIdAsync(SubjectId))
                .ReturnsAsync(() => user);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.FamilyName, user.LastName)
            };

            _mockUserClaimsPrincipalFactory
                .Setup(x => x.CreateAsync(user))
                .ReturnsAsync(
                    () =>
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                claims
                            )
                        )
                );

            _mockProfileDataRequestContext.Object.IssuedClaims = new List<Claim>();

            var requestedClaimTypes = new List<string>();

            foreach (var claim in claims)
            {
                requestedClaimTypes.Add(claim.Type);
            }

            _mockProfileDataRequestContext.Object.RequestedClaimTypes = requestedClaimTypes;

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act
            await identityProfileService.GetProfileDataAsync(_mockProfileDataRequestContext.Object);

            // Assert
            foreach (var claim in claims)
            {
                var issuedClaim = _mockProfileDataRequestContext.Object.IssuedClaims.FirstOrDefault(x => x.Type == claim.Type);

                Assert.NotNull(issuedClaim);
                Assert.Equal(claim.Value, issuedClaim.Value);
            }
        }

        [Fact]
        public async Task IsActiveAsync_ThrowsArgumentNullException_WhenSubjectIsNull()
        {
            // Arrange
            _mockIsActiveContext.Object.Subject = null;

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => identityProfileService.IsActiveAsync(_mockIsActiveContext.Object));
            Assert.Equal(nameof(_mockIsActiveContext.Object.Subject), exception.ParamName);
        }

        [Fact]
        public async Task IsActiveAsync_ThrowsException_WhenUserIsNull()
        {
            // Arrange
            _mockIsActiveContext.Object.Subject = GetSubject();

            _mockUserManager
                .Setup(x => x.FindByIdAsync(SubjectId))
                .ReturnsAsync(() => null);

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => identityProfileService.IsActiveAsync(_mockIsActiveContext.Object));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task IsActiveAsync_Success()
        {
            // Arrange
            _mockIsActiveContext.Object.Subject = GetSubject();

            var user = GetApplicationUser(nameof(IsActiveAsync_Success));

            _mockUserManager
                .Setup(x => x.FindByIdAsync(SubjectId))
                .ReturnsAsync(() => user);

            var identityProfileService = new IdentityProfileService(_mockUserManager.Object, _mockUserClaimsPrincipalFactory.Object);

            // Act
            await identityProfileService.IsActiveAsync(_mockIsActiveContext.Object);

            // Assert
            Assert.Equal(true, _mockIsActiveContext.Object.IsActive);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aurochses.Xunit;
using Aurochses.Xunit.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests.Controllers
{
    public class EmailConfirmationControllerTests : IClassFixture<TestServerWithDatabaseFixture>
    {
        private readonly TestServerWithDatabaseFixture _fixture;

        public EmailConfirmationControllerTests(TestServerWithDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Index_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpResponseMessage> GetEmailConfirmationResponse()
        {
            var getResponse = await _fixture.Client.GetAsync("/EmailConfirmation");

            getResponse.EnsureSuccessStatusCode();

            return getResponse;
        }

        private static Dictionary<string, string> GetEmailConfirmationFormData(string email)
        {
            return new Dictionary<string, string>
            {
                {"Email", email}
            };
        }

        [Fact]
        public async Task IndexPost_RedirectToUserNotFound_WhenUserNotFound()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(IndexPost_RedirectToUserNotFound_WhenUserNotFound));

            var request = new HttpRequestMessage(HttpMethod.Post, "/EmailConfirmation");
            await request.SetupAsync(GetEmailConfirmationFormData(email), await GetEmailConfirmationResponse());

            // Act
            var response = await _fixture.Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/UserNotFound", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task IndexPost_RedirectToIsAlreadyConfirmed_WhenIsAlreadyConfirmed()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(IndexPost_RedirectToIsAlreadyConfirmed_WhenIsAlreadyConfirmed));

            await _fixture.AddUser(email);

            var request = new HttpRequestMessage(HttpMethod.Post, "/EmailConfirmation");
            await request.SetupAsync(GetEmailConfirmationFormData(email), await GetEmailConfirmationResponse());

            // Act
            var response = await _fixture.Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/IsAlreadyConfirmed", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task IndexPost_RedirectToEmailSent()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(IndexPost_RedirectToEmailSent));

            await _fixture.AddUser(email, emailConfirmed: false);

            var request = new HttpRequestMessage(HttpMethod.Post, "/EmailConfirmation");
            await request.SetupAsync(GetEmailConfirmationFormData(email), await GetEmailConfirmationResponse());

            // Act
            var response = await _fixture.Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/EmailSent", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task UserNotFound_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation/UserNotFound");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task IsAlreadyConfirmed_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation/IsAlreadyConfirmed");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EmailSent_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation/EmailSent");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        private static string GetConfirmRequestUri(string userId, string token)
        {
            var queryStringParameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                queryStringParameters.Add("userId", userId);
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                queryStringParameters.Add("token", token);
            }

            return QueryHelpers.AddQueryString("/EmailConfirmation/Confirm", queryStringParameters);
        }

        [Fact]
        public async Task Confirm_RedirectToError_WhenUserIdIsNull()
        {
            // Arrange & Act
            var response = await _fixture.Client.GetAsync(GetConfirmRequestUri(null, "token"));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/Error", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Confirm_RedirectToError_WhenTokenIsNull()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(Confirm_RedirectToError_WhenTokenIsNull));

            var user = await _fixture.AddUser(email, emailConfirmed: false);

            // Act
            var response = await _fixture.Client.GetAsync(GetConfirmRequestUri(user.Id.ToString(), null));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/Error", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Confirm_RedirectToError_WhenUserNotFound()
        {
            // Arrange & Act
            var response = await _fixture.Client.GetAsync(GetConfirmRequestUri(Guid.Empty.ToString(), "token"));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/Error", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Confirm_RedirectToSuccess()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(Confirm_RedirectToSuccess));

            var user = await _fixture.AddUser(email, emailConfirmed: false);
            var token = await _fixture.UserManager.GenerateEmailConfirmationTokenAsync(user);

            // Act
            var response = await _fixture.Client.GetAsync(GetConfirmRequestUri(user.Id.ToString(), token));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/Success", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Confirm_RedirectToError_WhenConfirmEmailFailed()
        {
            // Arrange
            var email = GetType().GenerateEmail(nameof(Confirm_RedirectToError_WhenConfirmEmailFailed));

            var user = await _fixture.AddUser(email, emailConfirmed: false);

            // Act
            var response = await _fixture.Client.GetAsync(GetConfirmRequestUri(user.Id.ToString(), "token"));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/EmailConfirmation/Error", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Success_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation/Success");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Error_Return_View()
        {
            // Arrange
            var response = await _fixture.Client.GetAsync("/EmailConfirmation/Error");

            // Act & Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
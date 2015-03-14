// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Xunit;

namespace Microsoft.AspNet.Mvc.FunctionalTests
{
    public class TempDataTest
    {
        private const string SiteName = nameof(TempDataWebSite);
        private readonly Action<IApplicationBuilder> _app = new TempDataWebSite.Startup().Configure;

        [Fact]
        public async Task ViewRendersTempData()
        {
            // Arrange
            var server = TestHelper.CreateServer(_app, SiteName);
            var client = server.CreateClient();
            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("value", "Foo"),
            };
            var content = new FormUrlEncodedContent(nameValueCollection);

            // Act
            var response = await client.PostAsync("http://localhost/Home/DisplayTempData", content);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal("Foo", body);
        }

        [Fact]
        public async Task Redirect_RetainsTempData()
        {
            // Arrange
            var server = TestHelper.CreateServer(_app, SiteName);
            var client = server.CreateClient();
            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("value", "Foo"),
            };
            var content = new FormUrlEncodedContent(nameValueCollection);

            // Act 1
            var response = await client.PostAsync("/Home/SetTempDataAndRedirect", content);

            // Assert 1
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Act 2
            response = await client.SendAsync(GetRequest(response.Headers.Location.ToString(), response));

            // Assert 2
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal("Foo", body);
        }

        [Fact]
        public async Task Peek_RetainsTempData()
        {
            // Arrange
            var server = TestHelper.CreateServer(_app, SiteName);
            var client = server.CreateClient();
            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("value", "Foo"),
            };
            var content = new FormUrlEncodedContent(nameValueCollection);

            // Act 1
            var response = await client.PostAsync("/Home/SetTempDataAndRedirectToPeek", content);

            // Assert 1
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            // Act 2
            response = await client.SendAsync(GetRequest(response.Headers.Location.ToString(), response));

            // Assert 2
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal("FooFoo", body);
        }

        private HttpRequestMessage GetRequest(string path, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("Cookie", response.Headers.GetValues("Set-Cookie"));
            return request;
        }
    }
}
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Framework.Internal;

namespace ActionResultsWebSite
{
    public class ActionResultsVerificationController : Controller
    {
        public IActionResult Index([FromBody]DummyClass test)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            return Content("Hello World!");
        }

        public IActionResult GetBadResult()
        {
            return new BadRequestResult();
        }

        public IActionResult GetCreatedRelative()
        {
            return Created("1", CreateDummy());
        }

        public IActionResult GetCreatedAbsolute()
        {
            return Created("/ActionResultsVerification/GetDummy/1", CreateDummy());
        }

        public IActionResult GetCreatedQualified()
        {
            return Created("http://localhost/ActionResultsVerification/GetDummy/1", CreateDummy());
        }

        public IActionResult GetCreatedUri()
        {
            return Created(new Uri("/ActionResultsVerification/GetDummy/1", UriKind.Relative), CreateDummy());
        }

        public IActionResult GetCreatedAtAction()
        {
            var values = new { id = 1 };
            return CreatedAtAction("GetDummy", "ActionResultsVerification", values, CreateDummy());
        }

        public IActionResult GetCreatedAtRoute()
        {
            var values = new { controller = "ActionResultsVerification", Action = "GetDummy", id = 1 };
            return CreatedAtRoute(null, values, CreateDummy());
        }

        public IActionResult GetCreatedAtRouteWithRouteName()
        {
            var values = new { controller = "ActionResultsVerification", Action = "GetDummy", id = 1 };
            return CreatedAtRoute("custom-route", values, CreateDummy());
        }

        public IActionResult GetContentResult()
        {
            return Content("content");
        }

        public IActionResult GetContentResultWithContentType()
        {
            return Content("content", "application/json");
        }

        public IActionResult GetContentResultWithContentTypeAndEncoding()
        {
            return Content("content", "application/json", Encoding.ASCII);
        }

        public IActionResult GetObjectResultWithNoContent()
        {
            var result = new ObjectResult(null);
            result.StatusCode = StatusCodes.Status201Created;
            return result;
        }

        public IActionResult GetNotFoundObjectResult()
        {
            return HttpNotFound(null);
        }

        public IActionResult GetNotFoundObjectResultWithContent()
        {
            return HttpNotFound(CreateDummy());
        }

        public IActionResult GetNotFoundObjectResultWithDisposableObject()
        {
            return HttpNotFound(CreateDisposableDummy());
        }

        public int GetDisposeCallCount()
        {
            return DisposableDummy.disposeCallCount;
        }

        public DummyClass GetDummy(int id)
        {
            return CreateDummy();
        }

        private DummyClass CreateDummy()
        {
            return new DummyClass()
            {
                SampleInt = 10,
                SampleString = "Foo"
            };
        }

        private DisposableDummy CreateDisposableDummy()
        {
            return new DisposableDummy();
        }

        private class DisposableDummy : IDisposable
        {
            public static int disposeCallCount = 0;

            public void Dispose()
            {
                disposeCallCount++;
            }
        }
    }
}
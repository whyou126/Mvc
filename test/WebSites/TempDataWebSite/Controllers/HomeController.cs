// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc;

namespace TempDataWebSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayTempData(string value)
        {
            TempData["key"] = value;
            return View();
        }

        public IActionResult SetTempDataAndRedirect(string value)
        {
            TempData["key"] = value;
            return RedirectToAction("GetTempData");
        }

        public string GetTempData()
        {
            var value = TempData["key"];
            return value.ToString();
        }

        public IActionResult SetTempDataAndRedirectToPeek(string value)
        {
            TempData["key"] = value;
            return RedirectToAction("PeekTempData");
        }

        public IActionResult PeekTempData()
        {
            var peekValue = TempData.Peek("key");
            var getValue = TempData["key"];
            return Content(peekValue.ToString() + getValue.ToString());
        }
    }
}

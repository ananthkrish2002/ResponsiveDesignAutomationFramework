﻿using Builder.Element;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Builder.Driver
{
    public interface WebDriver : MobileDriver
    {
        void Navigate(Uri url);

        string Url { get; }

        IWebDriver NativeDriver { get;}

        AndroidDriver<IWebElement> androidDriver { get;}

        string TestName { get;}

        void Screenshot();

        WebDriverWait WebDriverWait(TimeSpan waitTime);

        void SwitchToFrame(By elementlocator);

        WebElement FindElement(By elementLocator);

        WebElement FindElement(By primaryLocator, IList<By> secondaryLocatorList);

        IList<WebElement> FindElements(By elementLocator);

        void WaitForPageLoad();

        IJavaScriptExecutor JsExecutor();

        void TakeFailureScreenshot();


    }

}
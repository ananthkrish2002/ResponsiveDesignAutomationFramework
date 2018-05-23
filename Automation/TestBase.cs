﻿using Builder.Driver;
using Castle.Core.Internal;
using Entities;
using Entities.Pages.Redmart;
using Harness;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation
{
    public abstract class TestBase 
    {
        public PageNavigation Page;
        public string SuperUser;
        public string SuperPassword;
        public string TestName;
        public TestContext TestContext { get; set; }


        private static List<string> _browserQueue = null;

        [TestInitialize]
        public void Initialize()
        {
            ReadSettings.ReadConfig();
            var browser = BrowserAttribute;                    
            InititializeConfig(BrowserName.Chrome);
        }

        public void InititializeConfig(string browserName)
        {
            TestName = DriverManager.Instance.TestName;
            SuperUser = Configurations.userName;
            SuperPassword = Configurations.passWord;
            Page = UIAutomation.Page;
        }

        [TestCleanup]
        public void cleanup()
        {
            if ((TestContext.CurrentTestOutcome != UnitTestOutcome.Passed))
            {
                DriverManager.Instance.TakeFailureScreenshot();
            }

            if (Configurations.realDevice)
                DriverManager.Instance.androidDriver.Quit();
            else
                DriverManager.Instance.NativeDriver.Quit();
            DriverManager.Reset();
        }

        public List<string> BrowserAttribute
        {
            get
            {
                List<string> browserAttribute = GetAttributeString(typeof(BrowserName));
                if (browserAttribute == null)
                    return null;

                return browserAttribute;
            }
        }

        private List<string> GetAttributeString(Type attribute)
        {
            if (_browserQueue.IsNullOrEmpty())
            {
                IEnumerable<string> availableEnvironments = attribute.GetFields().Select(e => e.GetRawConstantValue().ToString());
                _browserQueue = TestCategories.Where(c => availableEnvironments.Any(e => e == c)).ToList();
            }

            return _browserQueue;
        }

        private IEnumerable<string> TestCategories
        {
            get
            {
                List<string> result = new List<string>();

                var method = GetType().GetMethod(TestContext.TestName);

                var attributeList = method.GetCustomAttributes(typeof(DataRowAttribute),true);

                foreach (object attribute in attributeList)
                {
                    DataRowAttribute categotyAttribute = attribute as DataRowAttribute;

                    result.AddRange(categotyAttribute.Data.Select(e => e.ToString()));
                }

                return result;
            }
        }
    }
}

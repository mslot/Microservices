using System;

namespace Microservice.Extensions.NugetFeedTest
{
    public class Test
    {
        public string NewFeature()
        {
            string s = "";
            return "new feature";
        }

        public string Release020()
        {
            return "020";
        }

        public string PullRequest()
        {
            return "2";
        }

        public string PullRequest2()
        {
            return "33";
        }

        public string MethodTestGitVersion()
        {
            return "1";
        }

        public string MethodNew()
        {
            string s = "new method";
            return s;
        }

        public string Method()
        {
            string s = "Shared Test Method";
            return s;
        }
    }
}

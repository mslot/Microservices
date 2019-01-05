using System;

namespace Microservice.Extensions.NugetFeedTest
{
    public class Test
    {
        public string PullRequest()
        {
            return "2";
        }

        public string PullRequest2()
        {
            return "3";
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

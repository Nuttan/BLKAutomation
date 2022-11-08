using System.Diagnostics;
using System.Reflection;
using Atlassian.Jira;
using BLKAutoFramework.Base;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using BLKAutoFramework.Utility;
using BoDi;
using OpenQA.Selenium;



namespace BLKSupportPortalDemo.Hooks
{

    [Binding]
    public class HookInitialize : TestInitializeHook
    {
        private readonly IObjectContainer _objectContainer;
        private IFeaturesConfiguration _featuresConfiguration;
        private readonly ParallelConfig _parallelConfig;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private ExtentTest? _currentScenarioName;
        private static ExtentTest? _featureName;
        private static ExtentReports? _extent;

        public HookInitialize(IObjectContainer objectContainer, ParallelConfig parallelConfig, FeatureContext featureContext, ScenarioContext scenarioContext) : base(parallelConfig, objectContainer)
        {
            _objectContainer = objectContainer;
            _parallelConfig = parallelConfig;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        [AfterStep]
        public async void AfterEachStep()
        {
            _featuresConfiguration = _objectContainer.Resolve<IFeaturesConfiguration>();
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _currentScenarioName!.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    _currentScenarioName!.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    _currentScenarioName!.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    _currentScenarioName!.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
            }
            else if (_scenarioContext.TestError != null)
            {
                var strDate = DateTime.Now.ToString("dd-MM-yyyy-(hh-mm-ss)");
                var basePath = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly()?.Location!).LocalPath);
                var projectRootPath = basePath!.Split(new string[] { "BLKSupportPortalDemo" }, StringSplitOptions.None)[0];
                var screenshotsPath = $"{projectRootPath}Output\\Screenshots\\{strDate}\\{_scenarioContext.StepContext.StepInfo.Text.Split(new string[] { "\"" }, StringSplitOptions.None)[0]}";
                if (!System.IO.Directory.Exists(screenshotsPath))
                {
                    System.IO.Directory.CreateDirectory(screenshotsPath);
                }

                ((ITakesScreenshot)_parallelConfig.Driver!)
                    .GetScreenshot().SaveAsFile(screenshotsPath + "Screenshot.png", ScreenshotImageFormat.Png);

                var mediaEntity =
                    _parallelConfig.CaptureScreenshotAndReturnModel(_scenarioContext.ScenarioInfo.Title.Trim());
                if (stepType == "Given")
                    _currentScenarioName!.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "When")
                    _currentScenarioName!.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);
                else if (stepType == "Then")
                    _currentScenarioName!.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, mediaEntity);

                //Creating JIRA issueBug
                var settings = new JiraRestClientSettings(){ EnableRequestTrace = true };
                IssueType issuetyp = new IssueType("10018", "Bug",false);
                var jira = Jira.CreateRestClient("https://engnuttan.atlassian.net",  _featuresConfiguration.JIRAUserName, _featuresConfiguration.JIRAUserAPIKey,null);
                var issue1 = jira.CreateIssue(_featuresConfiguration.JIRAProjectKey);
                issue1.Type= issuetyp;
                issue1.Summary = _scenarioContext.TestError.Message;
                issue1.Description = _scenarioContext.TestError.Message + _scenarioContext.TestError.StackTrace;
                issue1.SaveChanges();

                //Upload attachment to JIRA issue
                var issue = await jira.Issues.GetIssueAsync(issue1.Key.ToString());
                if (issue != null)
                {
                    var path = screenshotsPath + "Screenshot.png";
                    if (File.Exists(path))
                    {
                        var fileAsByteArray = File.ReadAllBytes(path);
                        var attachment = new UploadAttachmentInfo(_scenarioContext.TestError.Message+".png", fileAsByteArray);
                        await issue.AddAttachmentAsync(new UploadAttachmentInfo[] { attachment });
                        Console.WriteLine("Uploading successful");
                    }
                }
                else
                {
                    Console.WriteLine("The issue cannot be found!");
                }
            }
            else if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    _currentScenarioName!.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    _currentScenarioName!.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    _currentScenarioName!.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
            }
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {
            var basePath = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly()?.Location!).LocalPath);
            var projectRootPath = basePath!.Split(new string[] { "BLKSupportPortalDemo" }, StringSplitOptions.None)[0];
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter($"{projectRootPath}Output\\ExtentReport\\SeleniumWithSpecflow\\ExtentReport.html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            //Attach report to reporter
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }


        [BeforeScenario]
        public void Initialize()
        {
            _objectContainer.RegisterTypeAs<FeaturesConfiguration, IFeaturesConfiguration>();
            InitializeSettings();
            //Get feature Name
            _featureName = _extent!.CreateTest<Feature>(_featureContext.FeatureInfo.Title);

            //Create dynamic scenario name
            _currentScenarioName = _featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TestStop()
        {

            _parallelConfig.Driver!.Quit();
            var rootDrive = Path.GetPathRoot(Environment.SystemDirectory); // for getting primary drive 
            var userName = Environment.UserName; // for getting user name
            try
            {
                //var allChromeSessions = Process.GetProcessesByName("chrome");
                //foreach (var chromeOpenProcess in allChromeSessions)
                //{
                //    try
                //    {
                //        chromeOpenProcess.Kill();
                //    }
                //    finally
                //    {
                //        chromeOpenProcess.WaitForExit();
                //    }
                //}
                //var downloadedMessageInfo = new DirectoryInfo(rootDrive + "Users\\" + userName + "\\AppData\\Local\\Google\\Chrome\\User Data");
                //try
                //{
                //    foreach (var file in downloadedMessageInfo.GetFiles())
                //    {
                //        file.Delete();
                //    }
                //}
                //catch (FileNotFoundException e)
                //{
                //    if (e.Source != null)
                //        Console.WriteLine("IOException source: {0}", e.Source);
                //    throw;
                //}
            }
            catch (IOException ex)
            {
                if (ex.Source != null)
                    Console.WriteLine("IOException source: {0}", ex.Source);
                throw;
            }
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            _extent!.Flush();
        }
    }
}

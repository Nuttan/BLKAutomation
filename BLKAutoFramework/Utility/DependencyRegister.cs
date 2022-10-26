using BoDi;
using TechTalk.SpecFlow;
using BLKAutoFramework.Base;

namespace BLKAutoFramework.Utility
{
    [Binding]
    public class DependencyRegister
    {
        private readonly IObjectContainer _objectContainer;
        public DependencyRegister(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }
        public void InitializeDependencies()
        {

            _objectContainer.RegisterTypeAs<FeaturesConfiguration, IFeaturesConfiguration>();
            _objectContainer.RegisterTypeAs<FeatureContext, FeatureContext>();
            _objectContainer.RegisterTypeAs<ScenarioContext, ScenarioContext>();
            _objectContainer.RegisterTypeAs<ParallelConfig, ParallelConfig>();
        }
    }
}

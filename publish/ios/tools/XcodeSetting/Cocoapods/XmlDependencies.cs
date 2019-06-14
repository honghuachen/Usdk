using Google;

namespace GooglePlayServices
{
	internal class XmlDependencies
	{
		protected string dependencyType = "dependencies";

		protected virtual bool Read(string filename, Logger logger)
		{
			return false;
		}

		public virtual bool ReadAll(string configPath,Logger logger)
		{
			bool result = true;
			if (!this.Read(configPath, logger))
				{
					logger.Log(string.Format("Unable to read {0} from {1}.\n{0} in this file will be ignored.", this.dependencyType, configPath), LogLevel.Error);
					result = false;
				}
			return result;
		}
	}
}

#nullable enable
using System.Collections.Generic;

namespace Raele.BasicCombatNodes;

public class ConfigurationWarningsBuilder
{
    private List<string> Warnings = new List<string>();

    public ConfigurationWarningsBuilder MaybeAppend(bool condition, string warningMessage)
	{
		if (condition) {
			this.Warnings.Add(warningMessage);
		}
		return this;
	}

	public ConfigurationWarningsBuilder Concat(string[]? manyWarnings)
	{
		if (manyWarnings != null) {
			this.Warnings.AddRange(manyWarnings);
		}
		return this;
	}

	public string[] Build() => this.Warnings.ToArray();
}

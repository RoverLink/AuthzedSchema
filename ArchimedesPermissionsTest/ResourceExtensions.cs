﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchimedesPermissionsTest;

internal static class ResourceExtensions
{
	public static async Task<string> ReadResourceAsync(this Assembly assembly, string name)
	{
		// Determine path
		string resourcePath = name;
		// Format: "{Namespace}.{Folder}.{filename}.{Extension}"

		var resources = assembly.GetManifestResourceNames();
		resourcePath = resources.Single(str => str.EndsWith(name));

		await using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
		using StreamReader reader = new(stream);
		return await reader.ReadToEndAsync();
	}
}
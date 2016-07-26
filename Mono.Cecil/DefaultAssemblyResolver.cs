//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
//
// Licensed under the MIT/X11 license.
//

#if !PCL && !NET_CORE

using System;
using System.Collections.Generic;

namespace Mono.Cecil {

	class AssemblyResolver : BaseAssemblyResolver {}

	public class DefaultAssemblyResolver : BaseAssemblyResolver, IDisposable {

		readonly IDictionary<string, AssemblyDefinition> cache;

		public DefaultAssemblyResolver ()
		{
			cache = new Dictionary<string, AssemblyDefinition> (StringComparer.Ordinal);
		}

		public override AssemblyDefinition Resolve (AssemblyNameReference name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			AssemblyDefinition assembly;
			if (cache.TryGetValue (name.FullName, out assembly))
				return assembly;

			assembly = base.Resolve (name);
			cache [name.FullName] = assembly;

			return assembly;
		}

		protected void RegisterAssembly (AssemblyDefinition assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException ("assembly");

			var name = assembly.Name.FullName;
			if (cache.ContainsKey (name))
				return;

			cache [name] = assembly;
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			foreach (var assembly in cache.Values)
				assembly.Dispose ();

			cache.Clear ();
		}
	}
}

#endif

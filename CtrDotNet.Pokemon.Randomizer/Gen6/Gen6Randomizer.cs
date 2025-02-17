﻿using CtrDotNet.Pokemon.Randomization.Common;

namespace CtrDotNet.Pokemon.Randomization.Gen6
{
	public abstract partial class Gen6Randomizer : BaseRandomizer
	{
		protected Gen6Randomizer() { }
		protected Gen6Randomizer( int seed ) : base( seed ) { }
	}
}
﻿using System;
using System.IO;
using CtrDotNet.Pokemon.Game;
using CtrDotNet.Pokemon.Structures.RomFS.Gen6;

namespace CtrDotNet.Pokemon.Utility
{
	static class Assertions
	{
		public static void AssertVersion( GameVersion expected, GameVersion actual )
		{
			if ( actual != expected )
				throw new ArgumentException( $"Version mismatch. Expected version {expected} but got {actual}." );
		}

		public static void AssertLength<T>( uint expected, T[] array, bool exact = false )
		{
			if ( array.Length < expected )
				throw new InvalidDataException( $"Data too short. Expected {( exact ? "exactly" : "at least" )} {expected}, but got {array.Length}" );

			if ( exact && array.Length > expected )
				throw new InvalidDataException( $"Data too long. Expected exactly {expected}, but got {array.Length}" );
		}

		public static void AssertLength( int expected, EncounterWild[] array, bool exact = false ) => AssertLength( (uint) expected, array, exact );
	}
}
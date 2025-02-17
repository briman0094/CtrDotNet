﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CtrDotNet.Pokemon.Game;
using CtrDotNet.Pokemon.Reference;

namespace PokemonDataDump
{
	class Program
	{
		private static readonly Regex BlankLineRegex = new Regex( @"\[~ (\d+)\]" );

		private static string     OutPath;
		private static GameConfig Config;

		private static void Main()
		{
			Program.Initialize().Wait();
		}

		private static async Task Initialize()
		{
			try
			{
				if ( !Enum.TryParse<GameVersion>( Settings.GameType, out var gameType ) )
				{
					Console.WriteLine( "Could not parse game type" );
					return;
				}

				OutPath = Path.Combine( Directory.GetCurrentDirectory(), Settings.OutputPath );

				if ( !Directory.Exists( OutPath ) )
					Directory.CreateDirectory( OutPath );

				string romPath = Path.GetFullPath( Settings.RomPath );

				Config = new GameConfig( gameType );
				await Config.Initialize( romPath, Language.English );

				await Task.WhenAll( Program.DumpSpeciesNames(),
									Program.DumpAbilityNames(),
									Program.DumpItemNames(),
									Program.DumpMoveNames(),
									Program.DumpTypeNames() );
			}
			catch ( Exception e )
			{
				Console.WriteLine( "Error dumping data:" );
				Console.WriteLine( e );

				while ( Console.KeyAvailable )
					Console.ReadKey( true );

				Console.ReadKey( true );
			}
		}

		private static async Task DumpStringTable( TextNames tableName, string fileName )
		{
			var textFile = await Config.GetTextFile( tableName );

			if ( textFile == null )
			{
				Console.WriteLine( $"Could not find string table for text file {tableName}" );
				return;
			}

			var    strings  = textFile.Lines;
			string filePath = Path.Combine( OutPath, $"{fileName}.db" );

			File.Delete( filePath );

			using ( var fs = new FileStream( filePath, FileMode.Create, FileAccess.Write, FileShare.None ) )
			using ( var sw = new StreamWriter( fs ) )
			{
				var lines = strings.Select( ( s, i ) => $"{i}={s}" )
								   .Where( s => !BlankLineRegex.IsMatch( s ) );

				foreach ( var line in lines )
					await sw.WriteLineAsync( line );
			}
		}

		private static Task DumpSpeciesNames() => Program.DumpStringTable( TextNames.SpeciesNames, "Species" );
		private static Task DumpAbilityNames() => Program.DumpStringTable( TextNames.AbilityNames, "Abilities" );
		private static Task DumpItemNames() => Program.DumpStringTable( TextNames.ItemNames,       "Items" );
		private static Task DumpMoveNames() => Program.DumpStringTable( TextNames.MoveNames,       "Moves" );
		private static Task DumpTypeNames() => Program.DumpStringTable( TextNames.Types,           "PokemonTypes" );
	}
}
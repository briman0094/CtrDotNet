﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using CtrDotNet.Pokemon.Game;

namespace CtrDotNet.Pokemon.Structures.RomFS.Gen6
{
	[ SuppressMessage( "ReSharper", "AutoPropertyCanBeMadeGetOnly.Global" ) ]
	public class EncounterGift : Common.EncounterStatic
	{
		#region Common

		public override ushort Species { get; set; }
		public ushort Unused2 { get; set; }
		public byte Form { get; set; }
		public byte Level { get; set; }
		public sbyte Ability { get; set; }
		public sbyte Nature { get; set; }
		public byte Shiny { get; set; }
		public byte Unused9 { get; set; }
		public byte UnusedA { get; set; }
		public byte UnusedB { get; set; }
		public override short HeldItem { get; set; }
		public sbyte Gender { get; set; }
		public byte Unused22 { get; set; }
		public byte UnusedLast { get; set; }

		// ReSharper disable once InconsistentNaming
		public sbyte[] IVs { get; set; } = new sbyte[ 6 ];

		#endregion

		#region ORAS

		public byte Unused11 { get; set; }
		public short MetLocation { get; set; }
		public ushort Move { get; set; }
		public byte[] ContestStats { get; set; }

		#endregion

		public EncounterGift( GameVersion gameVersion ) : base( gameVersion ) { }

		protected override void ReadData( BinaryReader br )
		{
			this.Species = br.ReadUInt16();
			this.Unused2 = br.ReadUInt16();
			this.Form = br.ReadByte();
			this.Level = br.ReadByte();
			this.Shiny = br.ReadByte();
			this.Ability = br.ReadSByte();
			this.Nature = br.ReadSByte();
			this.Unused9 = br.ReadByte();
			this.UnusedA = br.ReadByte();
			this.UnusedB = br.ReadByte();
			this.HeldItem = (short) br.ReadInt32();
			this.Gender = br.ReadSByte();

			if ( this.GameVersion.IsORAS() )
			{
				this.Unused11 = br.ReadByte();
				this.MetLocation = br.ReadInt16();
				this.Move = br.ReadUInt16();
			}

			for ( int i = 0; i < 6; i++ )
				this.IVs[ i ] = br.ReadSByte();

			if ( this.GameVersion.IsORAS() )
			{
				this.ContestStats = br.ReadBytes( 6 );
				this.Unused22 = br.ReadByte();
			}

			this.UnusedLast = br.ReadByte();
		}

		protected override void WriteData( BinaryWriter bw )
		{
			bw.Write( this.Species );
			bw.Write( this.Unused2 );
			bw.Write( this.Form );
			bw.Write( this.Level );
			bw.Write( this.Shiny );
			bw.Write( this.Ability );
			bw.Write( this.Nature );
			bw.Write( this.Unused9 );
			bw.Write( this.UnusedA );
			bw.Write( this.UnusedB );
			bw.Write( this.HeldItem );
			bw.Write( this.Gender );

			if ( this.GameVersion.IsORAS() )
			{
				bw.Write( this.Unused11 );
				bw.Write( this.MetLocation );
				bw.Write( this.Move );
			}

			for ( int i = 0; i < 6; i++ )
				bw.Write( this.IVs[ i ] );

			if ( this.GameVersion.IsORAS() )
			{
				bw.Write( this.ContestStats );
				bw.Write( this.Unused22 );
			}

			bw.Write( this.UnusedLast );
		}
	}
}
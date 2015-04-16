using System.Collections.Generic;
using System;

public static class GlobalMembersBodyActions
{


	#define PI




	public static SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, int>> >> actionSets = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, int>> >>();
	public static SortedDictionary<int, bool> actionSetsStatus = new SortedDictionary<int, bool>();
}


internal static partial class DefineConstants
{
	public const int NUI_INITIALIZE_FLAG_USES_AUDIO = 0x10000000;
	public const int NUI_INITIALIZE_FLAG_USES_DEPTH_AND_PLAYER_INDEX = 0x00000001;
	public const int NUI_INITIALIZE_FLAG_USES_COLOR = 0x00000002;
	public const int NUI_INITIALIZE_FLAG_USES_SKELETON = 0x00000008;
	public const int NUI_INITIALIZE_FLAG_USES_DEPTH = 0x00000020;
	public const int NUI_INITIALIZE_FLAG_USES_HIGH_QUALITY_COLOR = 0x00000040;
	public const long NUI_INITIALIZE_DEFAULT_HARDWARE_THREAD = 0xFFFFFFFF;
	public const int FACILITY_NUI = 0x301;
	public const double PI = 3.1415926535897932384;
}